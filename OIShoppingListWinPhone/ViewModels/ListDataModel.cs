using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LocalDatabaseSample.Model
{
    [Table]
    public class ShoppingList : INotifyPropertyChanged, INotifyPropertyChanging
    {
        //
        // TODO: Add columns and associations, as applicable, here.
        //
        public List<ShoppingListElement> listElements = new List<ShoppingListElement>();

        // Define ID: private field, public property, and database column.
        private int _listId;

        [Column (IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ListId
        {
            get { return _listId; }
            set
            {
                NotifyPropertyChanging("Id");
                _listId = value;
                NotifyPropertyChanged("Id");
            }
        }
                
        // Define item name: private field, public property, and database column.
        private string _listName;

        [Column]
        public string ListName
        {
            get { return _listName; }
            set
            {
                if (_listName != value)
                {
                    NotifyPropertyChanging("ListName");
                    _listName = value;
                    NotifyPropertyChanged("ListName");
                }
            }
        }             
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class ShoppingListElement : INotifyPropertyChanged, INotifyPropertyChanging
    {
        //
        // TODO: Add columns and associations, as applicable, here.
        //

        // Define ID: private field, public property, and database column.
        private int _Id;

        [Column (IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    NotifyPropertyChanging("Id");
                    _Id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        
        // Define category name: private field, public property, and database column.
        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        // Define category note: private field, public property, and database column.
        private string _note;

        [Column]
        public string Note
        {
            get { return _note; }
            set
            {
                NotifyPropertyChanging("Note");
                _note = value;
                NotifyPropertyChanged("Note");
            }
        }

        // Define completion value: private field, public property, and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
                
        private int _listId;

        [Column]
        public int ListId
        {
            get { return _listId; }
            set
            {
                if (_listId != value)
                {
                    NotifyPropertyChanging("Id");
                    _listId = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class ShoppingListDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public ShoppingListDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the Lists.
        public Table<ShoppingList> Lists;

        // Specify a table for the Lists' elements.
        public Table<ShoppingListElement> ListElements;
    }
}
