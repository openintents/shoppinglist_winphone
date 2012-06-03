using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// Directive for the data model.
using LocalDatabaseSample.Model;


namespace LocalDatabaseSample.ViewModel
{
    public class ShoppingListViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private ShoppingListDataContext listDB;

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        // Class constructor, create the data context object.
        public ShoppingListViewModel(string toDoDBConnectionString)
        {
            listDB = new ShoppingListDataContext(toDoDBConnectionString);
        }

        //
        // TODO: Add collections, list, and methods here.
        //

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            listDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        // All lists.
        private ObservableCollection<ShoppingList> _shoppingLists;
        public ObservableCollection<ShoppingList> ShoppingLists
        {
            get { return _shoppingLists; }
            set
            {
                _shoppingLists = value;
                NotifyPropertyChanged("ShoppingLists");
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadData()
        {

            // Specify the query for all to-do items in the database.
            var listsInDB = from ShoppingList list in listDB.Lists
                                select list;

            // Query the database and load all lists.
            ShoppingLists = new ObservableCollection<ShoppingList>(listsInDB);

            // Specify the query for all lists' elements in the database.
            foreach (ShoppingList list in ShoppingLists)
            {
                var listElementsInDB = from ShoppingListElement element in listDB.ListElements
                                       where element.ListId == list.ListId
                                       select element;
                List<ShoppingListElement> ListElements =
                    new List<ShoppingListElement>(listElementsInDB);
                list.listElements = ListElements;
            }

            this.IsDataLoaded = true;
        }
                
        // Add ShoppingList to the database and collections.
        public void AddNewShoppingList(ShoppingList newShoppingList)
        {
            // Add ShoppingList to the data context.
            listDB.Lists.InsertOnSubmit(newShoppingList);

            // Save changes to the database.
            listDB.SubmitChanges();

            // Add ShoppingList to the "all" observable collection.
            ShoppingLists.Add(newShoppingList);
        }

        // Add ShoppingListElement to the database and collections.
        public void AddNewShoppingListElement(ShoppingList shopList, ShoppingListElement newShoppingList)
        {
            // Add ShoppingList to the data context.
            listDB.ListElements.InsertOnSubmit(newShoppingList);
            shopList.listElements.Add(newShoppingList);
            // Save changes to the database.
            listDB.SubmitChanges();
        }

        
        // Remove a ShoppingList from the database and collections.
        public void DeleteShoppingList(ShoppingList listDelete)
        {
            foreach (ShoppingListElement elem in listDelete.listElements)
            {
                listDB.ListElements.DeleteOnSubmit(elem);
            }
            // Remove the to-do item from the "all" observable collection.
            ShoppingLists.Remove(listDelete);

            // Remove the to-do item from the data context.
            listDB.Lists.DeleteOnSubmit(listDelete);
                        
            // Save changes to the database.
            listDB.SubmitChanges();
        }

        // Remove a ShoppingListElement from the database and collections.
        public void DeleteShoppingListElement(ShoppingList shopList, ShoppingListElement listDelete)
        {
            listDB.ListElements.DeleteOnSubmit(listDelete);
            
            // Remove the to-do item from the "all" observable collection.
            shopList.listElements.Remove(listDelete);

            // Save changes to the database.
            listDB.SubmitChanges();
        }

        public void SubmitChecking(ShoppingList chList, ShoppingListElement chElement, bool bIsChecked)
        {
            listDB.ListElements.DeleteOnSubmit(chElement);
            listDB.SubmitChanges();

            ShoppingListElement newElement = new ShoppingListElement
            {
                ListId = chList.ListId,
                IsComplete = bIsChecked,
                Name = chElement.Name,
                Note = chElement.Note
            };
            listDB.ListElements.InsertOnSubmit(newElement);
            chList.listElements.Remove(chElement);
            chList.listElements.Add(newElement);

            // Save changes to the database.
            listDB.SubmitChanges();
        }
    }
}
