using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace OIShoppingListWinPhone.Settings
{
    public class ApplicationSettings
    {
        //ApplicationSettings class is a Singleton class.
        //It's necessary, because the application need only one instance of settings.
        //This field provides a global point of access to the Application Settings instance
        private static ApplicationSettings instance;

        //Instance of Application Settings
        private static IsolatedStorageSettings currentSettings;

        //Private const fields for Settings Keys
        private const string SelectedListIndexSettingKeyName = "SelectedListIndex";

        private const string FontSizeSettingKeyName = "FontSize";
        private const string SortOrderSettingKeyName = "SortOrder";

        private const string ShowPriceSettingKeyName = "ShowPrice";
        private const string ShowTagsSettingKeyName = "ShowTags";
        private const string ShowUnitsSettingKeyName = "ShowUnits";
        private const string ShowQuantitySettingKeyName = "ShowQuantity";
        private const string ShowPrioritySettingKeyName = "ShowPriority";

        private const string HideCheckedItemsSettingKeyName = "HideCheckedItems";
        private const string ShakeToCleanUpSettingKeyName = "ShakeToCleanUp";
        private const string TrackPerStorePricesSettingKeyName = "TrackPerStorePrices";
        private const string QuickModeEditSettingKeyName = "QuickModeEdit";
        private const string FiltersSettingKeyName = "Filters";
        private const string ResetQuantitySettingKeyName = "ResetQuantity";

        private const string AlwaysSameSortOrderSettingKeyName = "AlwaysSameSortOrder";
        private const string SortOdrerPickItemsSettingKeyName = "SortOrderPickItems";
        private const string PickItemsDirectlyInListSettingKeyName = "PickItemsDirectlyInList";
        //End of const fielвы for Settings Keys

        //Default Application Settings
        private const int SelectedListIndexSettingDefault = 0;

        private const int FontSizeSettingDefault = (int)FontSizeSettings.Default;
        private const int SortOrderSettingDefault = (int)SortOrderSettings.OldestFirst;

        private const bool ShowPriceSettingDefault = true;
        private const bool ShowTagsSettingDefault = true;
        private const bool ShowUnitsSettingDefault = true;
        private const bool ShowQuantitySettingDefault = true;
        private const bool ShowPrioritySettingDefault = true;

        private const bool HideCheckedItemsSettingDefault = false;
        private const bool ShakeToCleanUpSettingDefault = false;
        private const bool TrackPerStorePricesSettingDefault = false;
        private const bool QuickModeEditSettingDefault = false;
        private const bool FiltersSettingDefault = false;
        private const bool ResetQuantitySettingDefault = false;

        private const bool AlwaysSameSortOrderSettingDefault = false;
        private const int SortOrderPickItemsSettingDefault = (int)SortOrderSettings.Alphabetical;
        private const bool PickItemsDirectlyInListSettingDefault = false;
        //End of default Application Settings

        /// <summary>
        /// Enumeration for Font Size settings.
        /// </summary>
        public enum FontSizeSettings
        {
            Tiny = 20,
            Small = 30,
            Default = 45,
            Large = 64
        }

        /// <summary>
        /// Enumeration for Sort Order settings.
        /// </summary>
        public enum SortOrderSettings
        {
            UncheckedFirst_Alphabetical,
            Alphabetical,
            NewestFirst,
            OldestFirst,
            TagsAlphabetical,
            Priority_TagsAlphabetical,
            MostExpensiveFirst,
            UncheckedFirst_TagsAlphabetical,
            UncheckedFirst_Priority_Alphabetical,
            UnckeckedFirst_Priority_TagsAlphabetical
        }

        /// <summary>
        /// Constructor that loads the application settings from isolated storage
        /// (This parameterless constructor also requied for using data binding in XAML).
        /// </summary>
        public ApplicationSettings()
        {
            // Get the settings for the application.
            currentSettings = IsolatedStorageSettings.ApplicationSettings;
        }
        
        /// <summary>
        /// Load current instance of application settings for global access.
        /// </summary>
        /// <returns>The current instance of application settings</returns>
        public static ApplicationSettings Load()
        {
            if (instance == null)
                instance = new ApplicationSettings();

            return instance;
        }

        /// <summary>
        /// Set all Application settings to default values
        /// </summary>
        public void SetAllSettingsToDefault()
        {
            this.FontSizeSetting = FontSizeSettingDefault;
            this.SortOrderSetting = SortOrderSettingDefault;

            this.ShowPriceSettings = ShowPriceSettingDefault;
            this.ShowTagsSettings = ShowTagsSettingDefault;
            this.ShowUnitsSettings = ShowUnitsSettingDefault;
            this.ShowQuantitySettings = ShowQuantitySettingDefault;
            this.ShowPrioritySettings = ShowPrioritySettingDefault;

            this.HideCheckedItemsSettings = HideCheckedItemsSettingDefault;
            this.ShakeToCleanUpSettings = ShakeToCleanUpSettingDefault;
            this.TrackPerStorePricesSettings = TrackPerStorePricesSettingDefault;
            this.QuickEditModeSettings = QuickModeEditSettingDefault;
            this.FiltersSettings = FiltersSettingDefault;
            this.ResetQuantitySettings = ResetQuantitySettingDefault;

            this.AlwaysSameSortOrderSetting = AlwaysSameSortOrderSettingDefault;
            this.SortOrderPickItemsSetting = SortOrderPickItemsSettingDefault;
            this.PickItemsDirectlyInListSetting = PickItemsDirectlyInListSettingDefault;
        }

        /// <summary>
        /// Update a setting value for the application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (currentSettings.Contains(Key))
            {
                // If the value has changed
                if (currentSettings[Key] != value)
                {
                    // Store the new value
                    currentSettings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                currentSettings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (currentSettings.Contains(Key))
            {
                value = (T)currentSettings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            currentSettings.Save();
        }
        
        #region Application Settings

        /// <summary>
        /// Property to get and set a SlectedListIndex Key.
        /// </summary>
        public int SelectedListIndexSetting
        {
            get { return GetValueOrDefault<int>(SelectedListIndexSettingKeyName, SelectedListIndexSettingDefault); }
            set
            {
                if (AddOrUpdateValue(SelectedListIndexSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a FontSizeSetting Key.
        /// </summary>
        public int FontSizeSetting
        {
            get { return GetValueOrDefault<int>(FontSizeSettingKeyName, FontSizeSettingDefault); }
            set
            {
                if (AddOrUpdateValue(FontSizeSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a SortOrderSetting Key.
        /// </summary>
        public int SortOrderSetting
        {
            get { return GetValueOrDefault<int>(SortOrderSettingKeyName, SortOrderSettingDefault); }
            set
            {
                if (AddOrUpdateValue(SortOrderSettingKeyName, value))
                    Save();
            }
        }

        #endregion

        #region General Settings

        /// <summary>
        /// Property to get and set a HideCheckedItems Setting Key.
        /// </summary>
        public bool HideCheckedItemsSettings
        {
            get { return GetValueOrDefault<bool>(HideCheckedItemsSettingKeyName, HideCheckedItemsSettingDefault); }
            set
            {
                if (AddOrUpdateValue(HideCheckedItemsSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a ShakeToCleanUp Setting Key.
        /// </summary>
        public bool ShakeToCleanUpSettings
        {
            get { return GetValueOrDefault<bool>(ShakeToCleanUpSettingKeyName, ShakeToCleanUpSettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShakeToCleanUpSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a TrackPerStorePrices Setting Key.
        /// </summary>
        public bool TrackPerStorePricesSettings
        {
            get { return GetValueOrDefault<bool>(TrackPerStorePricesSettingKeyName, TrackPerStorePricesSettingDefault); }
            set
            {
                if (AddOrUpdateValue(TrackPerStorePricesSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a QuickEditMode Setting Key.
        /// </summary>
        public bool QuickEditModeSettings
        {
            get { return GetValueOrDefault<bool>(QuickModeEditSettingKeyName, QuickModeEditSettingDefault); }
            set
            {
                if (AddOrUpdateValue(QuickModeEditSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a Filters Setting Key.
        /// </summary>
        public bool FiltersSettings
        {
            get { return GetValueOrDefault<bool>(FiltersSettingKeyName, FiltersSettingDefault); }
            set
            {
                if (AddOrUpdateValue(FiltersSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a ResetQuantity Setting Key.
        /// </summary>
        public bool ResetQuantitySettings
        {
            get { return GetValueOrDefault<bool>(ResetQuantitySettingKeyName, ResetQuantitySettingDefault); }
            set
            {
                if (AddOrUpdateValue(ResetQuantitySettingKeyName, value))
                    Save();
            }
        }

        #endregion

        #region Appearance Settings

        /// <summary>
        /// Property to get and set a Show Price Setting Key.
        /// </summary>
        public bool ShowPriceSettings
        {
            get { return GetValueOrDefault<bool>(ShowPriceSettingKeyName, ShowPriceSettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShowPriceSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a Show Tags Setting Key.
        /// </summary>
        public bool ShowTagsSettings
        {
            get { return GetValueOrDefault<bool>(ShowTagsSettingKeyName, ShowTagsSettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShowTagsSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a Show Units Setting Key.
        /// </summary>
        public bool ShowUnitsSettings
        {
            get { return GetValueOrDefault<bool>(ShowUnitsSettingKeyName, ShowUnitsSettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShowUnitsSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a Show Quantity Setting Key.
        /// </summary>
        public bool ShowQuantitySettings
        {
            get { return GetValueOrDefault<bool>(ShowQuantitySettingKeyName, ShowQuantitySettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShowQuantitySettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a Show Priority Setting Key.
        /// </summary>
        public bool ShowPrioritySettings
        {
            get { return GetValueOrDefault<bool>(ShowPrioritySettingKeyName, ShowPrioritySettingDefault); }
            set
            {
                if (AddOrUpdateValue(ShowPrioritySettingKeyName, value))
                    Save();
            }
        }

        #endregion

        #region Pick Items Settings

        /// <summary>
        /// Property to get and set a AlwaysSameSortOrder Setting Key.
        /// </summary>
        public bool AlwaysSameSortOrderSetting
        {
            get { return GetValueOrDefault<bool>(AlwaysSameSortOrderSettingKeyName, AlwaysSameSortOrderSettingDefault); }
            set
            {
                if (AddOrUpdateValue(AlwaysSameSortOrderSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a SortOrderPickItems Setting Key.
        /// </summary>
        public int SortOrderPickItemsSetting
        {
            get { return GetValueOrDefault<int>(SortOdrerPickItemsSettingKeyName, SortOrderPickItemsSettingDefault); }
            set
            {
                if (AddOrUpdateValue(SortOdrerPickItemsSettingKeyName, value))
                    Save();
            }
        }

        /// <summary>
        /// Property to get and set a PickItemsDirectlyInList Setting Key.
        /// </summary>
        public bool PickItemsDirectlyInListSetting
        {
            get { return GetValueOrDefault<bool>(PickItemsDirectlyInListSettingKeyName,
                PickItemsDirectlyInListSettingDefault); }
            set
            {
                if (AddOrUpdateValue(PickItemsDirectlyInListSettingKeyName, value))
                    Save();
            }
        }

        #endregion
    }
}
