using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Linq;

using OIShoppingListWinPhone.DataModel;

namespace OIShoppingListWinPhone.ViewModel
{
    public sealed class ShoppingListViewModel : INotifyPropertyChanged
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
            
            if (listDB.DatabaseExists())
                listDB.DeleteDatabase();
            
            listDB.CreateDatabase();
        }
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

            this.IsDataLoaded = true;
        }

        public void AddNewList(ShoppingList newList)
        {
            newList.CreatedDate = DateTime.Now;
            newList.ModifiedDate = DateTime.Now;
            listDB.Lists.InsertOnSubmit(newList);
            listDB.SubmitChanges();
        }

        #region INotifyPropertyChanged members

        //Implementation for INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public sealed class ShoppingListDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public ShoppingListDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the Lists.
        public Table<ShoppingList> Lists;

        // Specify a table for the Lists' items.
        public Table<ShoppingListItem> ListItems;

        // Specify a table for the Lists' stores.
        public Table<ShoppingListStore> ListStores;

        // Specify a table for the Lists' itmes-stores.
        public Table<ShoppingListItemsStores> ItemsStores;
    }
}
