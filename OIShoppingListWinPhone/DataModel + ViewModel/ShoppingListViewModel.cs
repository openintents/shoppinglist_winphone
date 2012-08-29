using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Linq;

using OIShoppingListWinPhone.DataModel;
using System.Collections.Generic;

namespace OIShoppingListWinPhone.ViewModel
{
    public sealed class ShoppingListViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private ShoppingListDataContext listDB;

        /// <summary>
        /// Property for indicating whether the ShoppingLists collection was loaded
        /// </summary>
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        // Class constructor, create the data context object.
        public ShoppingListViewModel(string toDoDBConnectionString)
        {
            listDB = new ShoppingListDataContext(toDoDBConnectionString);

            if (!listDB.DatabaseExists())
                listDB.CreateDatabase();
        }

        // All lists collection.
        private ObservableCollection<ShoppingList> _shoppingLists;

        /// <summary>
        /// Observable collection of all ShoppingLists loaded from local database
        /// </summary>
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

        /// <summary>
        /// Adding new list to collection and save it to local database
        /// </summary>
        /// <param name="newList">Instance of new adding list</param>
        public void AddNewList(ShoppingList newList)
        {
            //Initialisation list fields associated with date
            newList.CreatedDate = DateTime.Now;
            newList.ModifiedDate = DateTime.Now;

            //Insetrion and submition all changes to database
            listDB.Lists.InsertOnSubmit(newList);
            listDB.SubmitChanges();
            //Adding new list to current observable collection
            ShoppingLists.Add(newList);
        }

        /// <summary>
        /// Delete list from database
        /// </summary>
        /// <param name="delList">List to be deleted</param>
        public void DeleteList(ShoppingList delList)
        {
            listDB.Lists.DeleteOnSubmit(delList);
            listDB.SubmitChanges();
            //Delete list from current loaded collection
            ShoppingLists.Remove(delList);
        }

        /// <summary>
        /// Rename list
        /// </summary>
        /// <param name="renameList">List to be renamed</param>
        /// <param name="newName">New list name</param>
        public void RenameList(ShoppingList renameList, string newName)
        {
            //Select corresponding list in data context collection and changing its name
            var listInDB = listDB.Lists.Where(c => c.ListID == renameList.ListID).FirstOrDefault();
            listInDB.ListName = newName;
            listInDB.ModifiedDate = DateTime.Now;
            //Submiting all changes to database
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Adding new list item to list
        /// </summary>
        /// <param name="currentList">Instance of list to which add new item</param>
        /// <param name="newListItem">Instance of new item to be added</param>
        public void AddNewListItem(ShoppingList currentList, ShoppingListItem newListItem)
        {
            newListItem.CreatedDate = DateTime.Now;
            newListItem.ModifiedDate = DateTime.Now;

            currentList.ListItems.Add(newListItem);
            currentList.ModifiedDate = DateTime.Now;

            //Filter list items collection with adding new item to list
            currentList.FilterItemsCollection();

            //Submiting all changes to database
            listDB.ListItems.InsertOnSubmit(newListItem);
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Move item to another list
        /// </summary>
        /// <param name="oldList">Instance of old item list</param>
        /// <param name="newList">Instance of new item list</param>
        /// <param name="listItem">Instance of item to be moved</param>
        public void MoveItemToAnotherList(ShoppingList oldList, ShoppingList newList, ShoppingListItem listItem)
        {

            oldList.ListItems.Remove(listItem);
            oldList.ModifiedDate = DateTime.Now;
            //Filter list items collection with adding new item to list
            oldList.FilterItemsCollection();

            listItem.List = newList;
            newList.ListItems.Add(listItem);
            newList.ModifiedDate = DateTime.Now;
            //Filter list items collection with adding new item to list
            newList.FilterItemsCollection();

            //Submiting all changes to database
            //listDB.ListItems.InsertOnSubmit(newListItem);
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Update list item status
        /// </summary>
        /// <param name="item">Instance of list item to be updated</param>
        /// <param name="status">New list item status</param>
        public void UpdateItemStatus(ShoppingListItem item, bool status)
        {
            item.Status = status ? 1 : 0;
            item.ModifiedDate = DateTime.Now;
            item.List.ModifiedDate = DateTime.Now;
            //After updating list item status it's necessary to sort items collection
            item.List.SortItemsCollection();
            //Submit all changes to database
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Change item status. If status was 'Unchecked' -> change to 'Checked',
        /// else if status was 'Checked' -> change to 'Unchecked'
        /// </summary>
        /// <param name="currentList">Instance of list containing item to be changed</param>
        /// <param name="item">Instance of item to be updated</param>
        public void ChangeItemStatus(ShoppingList currentList, ShoppingListItem item)
        {
            ShoppingListItem itemInDB = listDB.ListItems.Where(i => i.ItemID == item.ItemID).FirstOrDefault();
            if (itemInDB.Status == 0)
                itemInDB.Status = 1;
            else
                itemInDB.Status = 0;
            itemInDB.ModifiedDate = DateTime.Now;
            currentList.ModifiedDate = DateTime.Now;
            currentList.SortItemsCollection();

            listDB.SubmitChanges();
        }

        /// <summary>
        /// Set current item status to 'Picked'
        /// </summary>
        /// <param name="item">Instance of item to be picked</param>
        public void PickItem(ShoppingListItem item)
        {
            item.Status = 2;            
            listDB.SubmitChanges();
            item.List.FilterItemsCollection();
        }

        /// <summary>
        /// Update item priority
        /// </summary>
        /// <param name="item">Instance of item to be updated</param>
        /// <param name="priority">New item priority</param>
        public void UpdateItemPriority(ShoppingListItem item, int newPriority)
        {
            item.Priority = newPriority;
            item.ModifiedDate = DateTime.Now;
            item.List.ModifiedDate = DateTime.Now;

            item.List.SortItemsCollection();

            listDB.SubmitChanges();
        }

        /// <summary>
        /// Update item quantity
        /// </summary>
        /// <param name="item">Instance of item to be updated</param>
        /// <param name="newQuantity">New item quantity</param>
        public void UpdateItemQuantity(ShoppingListItem item, int? newQuantity)
        {
            float oldlPrice = item.Price;
            int? oldQuantity = item.Quantity;

            //Converting old item price to new (with corresponding item quantity)
            if (oldQuantity != null && oldQuantity != 0 && oldlPrice != 0.0)
                oldlPrice = oldlPrice / (float)oldQuantity;

            if (newQuantity != null)
                item.Price = oldlPrice * (float)newQuantity;
            else
                item.Price = oldlPrice;

            item.Quantity = newQuantity;
            item.ModifiedDate = DateTime.Now;
            item.List.ModifiedDate = DateTime.Now;

            item.List.SortItemsCollection();

            listDB.SubmitChanges();
        }

        /// <summary>
        /// Update item fields
        /// </summary>
        /// <param name="listID">List ID containing item to be updated</param>
        /// <param name="ID">ID of item to be updated</param>
        /// <param name="name">New item name</param>
        /// <param name="quantity">New item quantity</param>
        /// <param name="units">New item units</param>
        /// <param name="price">New item price</param>
        /// <param name="tags">New item tags</param>
        /// <param name="priority">New item priority</param>
        /// <param name="note">New item note</param>
        public void UpdateListItem(int listID,
            int ID,
            string name,
            int? quantity,
            string units,
            float price,
            string tags,
            int? priority,
            string note)
        {
            //Selecting certain item from current list items collection
            var itemInDB = listDB.ListItems.Where(c => c.ItemID == ID).FirstOrDefault();
            //Updating item fields
            itemInDB.ItemName = name;
            itemInDB.Quantity = quantity;
            itemInDB.Units = units;
            itemInDB.Price = price;
            itemInDB.Tag = tags;
            itemInDB.Priority = priority;
            itemInDB.Note = note;
            itemInDB.ModifiedDate = DateTime.Now;
            itemInDB.List.ModifiedDate = DateTime.Now;

            //Filtering collection after updating all item fields
            itemInDB.List.FilterItemsCollection();
            //Submit all changes to database
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Delete item from list permanently
        /// </summary>
        /// <param name="delItem">Item to be deleted permanently</param>
        public void DeleteListItem(ShoppingListItem delItem)
        {
            IEnumerable<ShoppingListItemsStores> itemsStores =
                        listDB.ItemsStores.Where(i => i.ItemID == delItem.ItemID);

            if (itemsStores != null)
                listDB.ItemsStores.DeleteAllOnSubmit(itemsStores);

            delItem.List.ModifiedDate = DateTime.Now;
            delItem.List.ListItems.Remove(delItem);
            delItem.List.FilterItemsCollection();
            listDB.ListItems.DeleteOnSubmit(delItem);
            listDB.SubmitChanges();
        }

        public void UpdateListFilterTag(ShoppingList list, string filterTag)
        {
            list.FilterTag = filterTag;
            listDB.SubmitChanges();
        }

        public void UpdateListFilterStore(ShoppingList list, string filterStore)
        {
            list.FilterStore = filterStore;
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Add new store to list
        /// </summary>
        /// <param name="list">Instance of list in which store will be added</param>
        /// <param name="store">Instance of store to be added</param>
        public void AddNewStore(ShoppingList list, ShoppingListStore store)
        {
            list.ListStores.Add(store);
            list.ModifiedDate = DateTime.Now;
            store.CreatedDate = DateTime.Now;
            store.ModifiedDate = DateTime.Now;

            listDB.ListStores.InsertOnSubmit(store);
            list.UpdateListStoreLabels();
            listDB.SubmitChanges();
        }

        /// <summary>
        /// Load stores per one list item
        /// </summary>
        /// <param name="itemId">Item ID stores of which need to return</param>
        /// <returns>IEnumerable collection of ShoppingListItemsStores</returns>
        public IEnumerable<ShoppingListItemsStores> LoadStoresPerItem(int itemId)
        {
            var ItemsStores = from itemsStores in listDB.ItemsStores
                              where itemsStores.ItemID == itemId
                              select itemsStores;
            return ItemsStores;
        }

        //Update many-to-many database relationship
        public void UpdateRelationship(int itemId, string storeName, bool isCheck, string aisle, float price)
        {
            ShoppingListItem item = listDB.ListItems.Where(c => c.ItemID == itemId).FirstOrDefault();
            ShoppingListStore store = listDB.ListStores.Where(x => x.StoreName == storeName).FirstOrDefault();

            ShoppingListItemsStores item_store = listDB.ItemsStores.Where(i => i.ItemID == item.ItemID &&
                i.StoreID == store.StoreID).FirstOrDefault();

            if (isCheck)
            {
                //Editing existing ItemsStores
                if (item_store != null)
                {
                    item_store.Aisle = aisle;
                    item_store.StorePrice = price;

                    listDB.SubmitChanges();
                }
                //Adding new ItemsStores
                else
                {
                    ShoppingListItemsStores itemstore = new ShoppingListItemsStores()
                        {
                            ListItem = item,
                            Store = store,
                            Aisle = aisle,
                            StorePrice = price
                        };
                    item.ItemsStores.Add(itemstore);
                    store.StoresItems.Add(itemstore);
                    listDB.ItemsStores.InsertOnSubmit(itemstore);
                }
            }
            //Deleting existing ItemsSotres
            else
            {
                if (item_store != null)
                    listDB.ItemsStores.DeleteOnSubmit(item_store);
            }
            item.UpdateItemPriceBindingTargets();
            listDB.SubmitChanges();
        }

        //Rename existing store
        public void RenameStore(string storeName, string newName)
        {
            ShoppingListStore store = listDB.ListStores.Where(s => s.StoreName == storeName).FirstOrDefault();
            if (store != null)
            {
                store.StoreName = newName;
                store.List.UpdateListStoreLabels();
                listDB.SubmitChanges();
            }
        }

        //Delete existing store
        public void DeleteStore(string storeName)
        {
            ShoppingListStore store = listDB.ListStores.Where(s => s.StoreName == storeName).FirstOrDefault();
            if (store != null)
            {
                IEnumerable<ShoppingListItemsStores> itemsStores =
                    listDB.ItemsStores.Where(s => s.StoreID == store.StoreID);

                if (itemsStores != null)
                    listDB.ItemsStores.DeleteAllOnSubmit(itemsStores);

                store.List.ListStores.Remove(store);
                store.List.UpdateListStoreLabels();
                listDB.ListStores.DeleteOnSubmit(store);
                listDB.SubmitChanges();
            }
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

    /// <summary>
    /// Class for implementing database data context
    /// </summary>
    public sealed class ShoppingListDataContext : DataContext
    {
        /// <summary>
        /// Pass the connection string to the base class.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public ShoppingListDataContext(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        /// Specify a table for the Lists.
        /// </summary>
        public Table<ShoppingList> Lists;

        /// <summary>
        /// Specify a table for the Lists' items.
        /// </summary>
        public Table<ShoppingListItem> ListItems;

        /// <summary>
        /// Specify a table for the Lists' stores.
        /// </summary>
        public Table<ShoppingListStore> ListStores;

        /// <summary>
        /// Specify a table for the Lists' itmes-stores.
        /// </summary>
        public Table<ShoppingListItemsStores> ItemsStores;
    }
}
