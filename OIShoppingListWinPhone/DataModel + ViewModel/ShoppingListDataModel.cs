using System;
using System.ComponentModel;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
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

        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "INT", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
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

        [Column(DbType = "NVARCHAR")]
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
                
        private EntitySet<ShoppingListItem> _listItems;

        [Association(Storage = "_listItems", OtherKey = "ListID", ThisKey = "ListID")]
        public EntitySet<ShoppingListItem> ListItems
        {
            get { return this._listItems; }
            set { _listItems.Assign(value); }
        }
                
        private EntitySet<ShoppingListStore> _listStores;

        [Association(Storage = "_listStores", OtherKey = "ListID", ThisKey = "ListID")]
        public EntitySet<ShoppingListStore> ListStores
        {
            get { return this._listStores; }
            set { this._listStores.Assign(value); }
        }

        public ShoppingList()
        {
            _listItems = new EntitySet<ShoppingListItem>();
            _listStores = new EntitySet<ShoppingListStore>();
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

        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "INT", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
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
        }
                
        private string _itemName;

        [Column(DbType = "NVARCHAR")]
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

        private Boolean _status;

        [Column (DbType="BIT")]
        public Boolean Status
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

        private Decimal _price;

        [Column(DbType = "DECIMAL")]
        public Decimal Price
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

        private int _quantity;

        [Column(DbType = "INT")]
        public int Quantity
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

        private int _units;

        [Column(DbType = "INT")]
        public int Units
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

        private int _priority;

        [Column(DbType = "INT")]
        public int Priority
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

        [Column(DbType="NVARCHAR")]
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

        [Column(DbType = "NVARCHAR")]
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

        private EntityRef<ShoppingList> _list;

        [Association (Storage="_list", ThisKey="ListID", OtherKey="ListID", IsForeignKey=true)]
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
        [Column (IsPrimaryKey=true, IsDbGenerated=false, DbType="INT", CanBeNull=false, AutoSync=AutoSync.OnInsert)]
        private int _storeId;

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
        }

        private string _storeName;

        [Column(DbType = "NVARCHAR")]
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

        [Association(Storage = "_list", ThisKey = "ListID", OtherKey = "ListID", IsForeignKey = true)]
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
