using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace OIShoppingListWinPhone.DataModel
{
    /// <summary>
    /// ShoppingList database table slass
    /// </summary>
    /// <returns></returns>
    [Table (Name="Lists")]
    public sealed class ShoppingList: INotifyPropertyChanging, INotifyPropertyChanged
    {        
        private int _listId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ListID
        {
            get { return this._listId; }
            set
            {
                if (this._listId != value)
                {
                    NotifyPropertyChanging("ListID");
                    this._listId = value;
                    NotifyPropertyChanged("ListID");
                }
            }
        }

        private string _listName;

        [Column(DbType = "NVARCHAR(100)")]
        public string ListName
        {
            get { return this._listName; }
            set
            {
                if (this._listName != value)
                {
                    NotifyPropertyChanging("ListName");
                    this._listName = value;
                    NotifyPropertyChanged("ListName");
                }
            }
        }
                
        private DateTime _createdDate;

        [Column(DbType = "DATETIME")]
        public DateTime CreatedDate
        {
            get { return this._createdDate; }
            set
            {
                if (this._createdDate != value)
                {
                    NotifyPropertyChanging("ListCreatedDate");
                    this._createdDate = value;
                    NotifyPropertyChanged("ListCreatedDate");
                }
            }
        }
                
        private DateTime _modifiedDate;

        [Column(DbType = "DATETIME")]
        public DateTime ModifiedDate
        {
            get { return this._modifiedDate; }
            set
            {
                if (this._modifiedDate != value)
                {
                    NotifyPropertyChanging("ListModifiedDate");
                    this._modifiedDate = value;
                    NotifyPropertyChanged("ListModifiedDate");
                }
            }
        }

        private int _checkedCount;
        public int CheckedCount
        {
            get { return this._checkedCount; }
            set
            {
                if (this._checkedCount != value)
                {
                    NotifyPropertyChanging("CheckedCount");
                    this._checkedCount = value;
                    NotifyPropertyChanged("CheckedCount");
                }
            }
        }

        private float _checkedPrice;
        public float CheckedPrice
        {
            get { return this._checkedPrice; }
            set
            {
                if (this._checkedPrice != value)
                {
                    NotifyPropertyChanging("CheckedPrice");
                    this._checkedPrice = value;
                    NotifyPropertyChanged("CheckedPrice");
                }
            }
        }

        private float _totalPrice;
        public float TotalPrice
        {
            get { return this._totalPrice; }
            set
            {
                if (this._totalPrice != value)
                {
                    NotifyPropertyChanging("TotalPrice");
                    this._totalPrice = value;
                    NotifyPropertyChanged("TotalPrice");
                }
            }
        }

        public List<string> Tags
        {
            get
            {
                List<string> tags = new List<string>();
                foreach (ShoppingListItem item in this.ListItems)
                {
                    foreach (string tag in item.Tags)
                    {
                        if (!tags.Contains(tag))
                            tags.Add(tag);
                    }
                }
                tags.Sort();
                tags.Insert(0, "[Empty]");
                return tags;
            }
        }
                
        private EntitySet<ShoppingListItem> _listItems;

        [Association(Storage = "_listItems", OtherKey = "ListID", ThisKey = "ListID", DeleteRule="CASCADE")]
        public EntitySet<ShoppingListItem> ListItems
        {
            get { return this._listItems; }
            set
            {
                _listItems.Assign(value);

                int chCount = 0;
                float chPrice = 0.0F;
                float totalPrice = 0.0F;

                foreach (ShoppingListItem item in _listItems)
                {
                    if (item.Status == (int)ShoppingListItem.StatusEnumerator.Checked)
                    {
                        chCount++;
                        chPrice += item.Price;
                    }
                    totalPrice += item.Price;
                }

                CheckedCount = chCount;
                CheckedPrice = chPrice;
                TotalPrice = totalPrice;
            }
        }
                
        private EntitySet<ShoppingListStore> _listStores;

        [Association(Storage = "_listStores", OtherKey = "ListID", ThisKey = "ListID", DeleteRule="CASCADE")]
        public EntitySet<ShoppingListStore> ListStores
        {
            get
            {
                this._listStores.Insert(0, new ShoppingListStore() { List = this, StoreName = "[Empty]" });
                return this._listStores;
            }
            set { this._listStores.Assign(value); }
        }

        public ShoppingList()
        {
            _listItems = new EntitySet<ShoppingListItem>();
            _listStores = new EntitySet<ShoppingListStore>();

            CheckedCount = 0;
            CheckedPrice = 0.0F;
            TotalPrice = 0.0F;
        }

        #region INotifyPropertyChanging members

        //Implementation for INotifyPropertyChanging interface
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(String propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (null != handler)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

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
    /// ShoppingListItem database table class
    /// </summary>
    /// <returns></returns>
    [Table (Name="Items")]
    public sealed class ShoppingListItem: INotifyPropertyChanging, INotifyPropertyChanged
    {        
        private int _itemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ItemID
        {
            get { return this._itemId; }
            set
            {
                if (this._itemId != value)
                {
                    NotifyPropertyChanging("ItemID");
                    this._itemId = value;
                    NotifyPropertyChanged("ItemID");
                }
            }
        }

        private int _listId;

        [Column]
        public int ListID
        {
            get { return this._listId; }
            private set
            {
                if (this._listId != value)
                {
                    NotifyPropertyChanging("ListID");
                    this._listId = value;
                    NotifyPropertyChanged("ListID");
                }
            }
        }
                
        private string _itemName;

        [Column(DbType = "NVARCHAR(100)")]
        public string ItemName
        {
            get { return this._itemName; }
            set
            {
                if (this._itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    this._itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        private int _status;

        [Column (DbType="INT")]
        public int Status
        {
            get { return this._status; }
            set
            {
                if (this._status != value)
                {
                    NotifyPropertyChanging("ItemStatus");
                    this._status = value;
                    NotifyPropertyChanged("ItemStatus");
                }
            }
        }

        public enum StatusEnumerator
        {
            Unchecked = 0,
            Checked = 1,
            Picked = 2
        }

        private float _price;

        [Column(DbType = "FLOAT")]
        public float Price
        {
            get { return this._price; }
            set
            {
                if (this._price!= value)
                {
                    NotifyPropertyChanging("ItemPrice");
                    this._price = value;
                    NotifyPropertyChanged("ItemPrice");
                }
            }
        }

        private int? _quantity;

        [Column(DbType = "INT", CanBeNull = true)]
        public int? Quantity
        {
            get { return this._quantity; }
            set
            {
                if (this._quantity != value)
                {
                    NotifyPropertyChanging("ItemQuantity");
                    this._quantity = value;
                    NotifyPropertyChanged("ItemQuantity");
                }
            }
        }

        private int? _units;

        [Column(DbType = "INT", CanBeNull = true)]
        public int? Units
        {
            get { return this._units; }
            set
            {
                if (this._units != value)
                {
                    NotifyPropertyChanging("ItemUnits");
                    this._units = value;
                    NotifyPropertyChanged("ItemUnits");
                }
            }
        }

        private int? _priority;

        [Column(DbType = "INT", CanBeNull = true)]
        public int? Priority
        {
            get { return this._priority; }
            set
            {
                if (this._priority != value)
                {
                    NotifyPropertyChanging("ItemPriority");
                    this._priority = value;
                    NotifyPropertyChanged("ItemPriority");
                }
            }
        }

        private string _tag;

        [Column(DbType = "NVARCHAR(100)")]
        public string Tag
        {
            get { return this._tag; }
            set
            {
                if (this._tag != value)
                {
                    NotifyPropertyChanging("ItemTag");
                    this._tag = value;
                    NotifyPropertyChanged("ItemTag");
                }
            }
        }

        private string _note;

        [Column(DbType = "NVARCHAR(100)")]
        public string Note
        {
            get { return this._note; }
            set
            {
                if (this._note != value)
                {
                    NotifyPropertyChanging("ItemNote");
                    this._note = value;
                    NotifyPropertyChanged("ItemNote");
                }
            }
        }
                
        private DateTime _createdDate;

        [Column(DbType = "DATETIME")]
        public DateTime CreatedDate
        {
            get { return this._createdDate; }
            set
            {
                if (this._createdDate != value)
                {
                    NotifyPropertyChanging("ListCreatedDate");
                    this._createdDate = value;
                    NotifyPropertyChanged("ListCreatedDate");
                }
            }
        }
                
        private DateTime _modifiedDate;

        [Column(DbType = "DATETIME")]
        public DateTime ModifiedDate
        {
            get { return this._modifiedDate; }
            set
            {
                if (this._modifiedDate != value)
                {
                    NotifyPropertyChanging("ListModifiedDate");
                    this._modifiedDate = value;
                    NotifyPropertyChanged("ListModifiedDate");
                }
            }
        }

        public List<string> Tags
        {
            get
            {
                List<string> tags = new List<string>();
                string[] tags_array = this.Tag.Split(',');
                foreach (string tag in tags_array)
                {
                    if(tag.Trim() != string.Empty)
                    tags.Add(tag.Trim());
                }
                return tags;
            }
        }

        private EntityRef<ShoppingList> _list;

        [Association (Storage="_list", ThisKey="ListID", OtherKey="ListID", IsForeignKey=true, DeleteRule="CASCADE")]
        public ShoppingList List
        {
            get { return _list.Entity; }
            set
            {
                this._list.Entity = value;
                if (value != null)
                    this._listId = value.ListID;
            }
        }

        private EntitySet<ShoppingListItemsStores> _itemsStores;

        [Association(Storage = "_itemsStores", OtherKey = "ItemID", ThisKey = "ItemID", DeleteRule="CASCADE")]
        public EntitySet<ShoppingListItemsStores> ItemsStores
        {
            get { return this._itemsStores; }
            set { this._itemsStores.Assign(value); }
        }

        public ShoppingListItem()
        {
            _itemsStores = new EntitySet<ShoppingListItemsStores>();
        }

        #region INotifyPropertyChanging members

        //Implementation for INotifyPropertyChanging interface
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(String propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (null != handler)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

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
    /// ShoppingListStore database table class
    /// </summary>
    /// <returns></returns>
    [Table (Name="Stores")]
    public sealed class ShoppingListStore: INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _storeId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int StoreID
        {
            get { return this._storeId; }
            set
            {
                if (this._storeId != value)
                {
                    NotifyPropertyChanging("StoreID");
                    this._storeId = value;
                    NotifyPropertyChanged("StoreID");
                }
            }
        }

        private int _listId;

        [Column]
        public int ListID
        {
            get { return this._listId; }
            private set
            {
                if (this._listId != value)
                {
                    NotifyPropertyChanging("ListID");
                    this._listId = value;
                    NotifyPropertyChanged("ListID");
                }
            }
        }

        private string _storeName;

        [Column(DbType = "NVARCHAR(100)")]
        public string StoreName
        {
            get { return this._storeName; }
            set
            {
                if (this._storeName != value)
                {
                    NotifyPropertyChanging("ListName");
                    this._storeName = value;
                    NotifyPropertyChanged("ListName");
                }
            }
        }

        private DateTime _createdDate;

        [Column(DbType = "DATETIME")]
        public DateTime CreatedDate
        {
            get { return this._createdDate; }
            set
            {
                if (this._createdDate != value)
                {
                    NotifyPropertyChanging("ListCreatedDate");
                    this._createdDate = value;
                    NotifyPropertyChanged("ListCreatedDate");
                }
            }
        }

        private DateTime _modifiedDate;

        [Column(DbType = "DATETIME")]
        public DateTime ModifiedDate
        {
            get { return this._modifiedDate; }
            set
            {
                if (this._modifiedDate != value)
                {
                    NotifyPropertyChanging("ListModifiedDate");
                    this._modifiedDate = value;
                    NotifyPropertyChanged("ListModifiedDate");
                }
            }
        }

        private EntityRef<ShoppingList> _list;

        [Association(Storage = "_list", ThisKey = "ListID", OtherKey = "ListID", IsForeignKey = true, DeleteRule="CASCADE")]
        public ShoppingList List
        {
            get { return _list.Entity; }
            set
            {
                this._list.Entity = value;
                if (value != null)
                    this._listId = value.ListID;
            }
        }

        private EntitySet<ShoppingListItemsStores> _storesItems;

        [Association(Storage = "_storesItems", OtherKey = "StoreID", ThisKey = "StoreID", DeleteRule="CASCADE")]
        public EntitySet<ShoppingListItemsStores> StoresItems
        {
            get { return this._storesItems; }
            set { this._storesItems.Assign(value); }
        }

        public ShoppingListStore()
        {
            _storesItems = new EntitySet<ShoppingListItemsStores>();
        }

        #region INotifyPropertyChanging members

        //Implementation for INotifyPropertyChanging interface
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(String propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (null != handler)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

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
    /// ShoppingListItemsStores database table class (for implementation many-to-many relationship).
    /// Each ListItem can be copied many times to ItemsStores; each ListStore can be copied many times to ItemsStores.
    /// </summary>
    /// <returns></returns>
    [Table(Name = "ItemsStores")]
    public sealed class ShoppingListItemsStores : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _Id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, DbType = "INT NOT NULL Identity", AutoSync = AutoSync.OnInsert)]
        public int ID
        {
            get { return this._Id; }
            set
            {
                if (this._Id != value)
                {
                    NotifyPropertyChanging("ItemsStoresID");
                    this._Id = value;
                    NotifyPropertyChanged("ItemsStoresID");
                }
            }
        }

        private int _itemId;

        [Column]
        public int ItemID
        {
            get { return this._itemId; }
            set
            {
                if (this._itemId != value)
                {
                    NotifyPropertyChanging("ItemID");
                    this._itemId = value;
                    NotifyPropertyChanged("ItemID");
                }
            }
        }

        private int _storeId;

        [Column]
        public int StoreID
        {
            get { return this._storeId; }
            set
            {
                if (this._storeId != value)
                {
                    NotifyPropertyChanging("StoreID");
                    this._storeId = value;
                    NotifyPropertyChanged("StoreID");
                }
            }
        }

        private EntityRef<ShoppingListItem> _item;

        [Association(Name = "FK_ItemsStores_List", Storage = "_item", ThisKey = "ItemID", OtherKey = "ItemID", IsForeignKey = true, DeleteRule = "CASCADE")]
        public ShoppingListItem ListItem
        {
            get { return this._item.Entity; }
            set
            {
                this._item.Entity = value;
                if (value != null)
                    this._itemId = value.ItemID;
            }
        }
                
        private EntityRef<ShoppingListStore> _store;

        [Association(Name = "FK_StoresItems_List", Storage = "_store", ThisKey = "StoreID", OtherKey = "StoreID", IsForeignKey = true, DeleteRule = "CASCADE")]
        public ShoppingListStore Store
        {
            get { return _store.Entity; }
            set
            {
                this._store.Entity = value;
                if (value != null)
                    this._storeId = value.StoreID;
            }
        }

        #region INotifyPropertyChanging members

        //Implementation for INotifyPropertyChanging interface
        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(String propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (null != handler)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

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
}
