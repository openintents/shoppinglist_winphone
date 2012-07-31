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
        private const string SortOrdeSettingKeyName = "SortOrder";

        private const string ShowPriceSettingKeyName = "ShowPrice";
        private const string ShowTagsSettingKeyName = "ShowTags";
        private const string ShowUnitsSettingKeyName = "ShowUnits";
        private const string ShowQuantitySettingKeyName = "ShowQuantity";
        private const string ShowPrioritySettingKeyName = "ShowPriority";

        private const string HideCheckedItemsSettingKeyName = "HideCheckedItems";
        private const string FastScrollingSettingKeyName = "FastScrolling";
        private const string ShakeToCleanUpSettingKeyName = "ShakeToCleanUp";
        private const string TrackPerStorePricesSettingKeyName = "TrackPerStorePrices";
        private const string DisableScreenLockSettingKeyName = "DisableScreenLock";
        private const string QuickModeEditSettingKeyName = "QuickModeEdit";
        private const string FiltersSettingKeyName = "Filters";
        private const string ResetQuantitySettingKeyName = "ResetQuantity";
        //End of const fielвы for Settings Keys

        //Default Application Settings
        private const int SelectedListIndexSettingDefault = 0;

        private const int FontSizeSettingDefault = (int)FontSizeSettings.Default;
        private const int SortOrderSettingDefault = (int)SortOrderSettings.OldestFirst;

        private const bool ShowPriceSettingDefault = false;
        private const bool ShowTagsSettingDefault = true;
        private const bool ShowUnitsSettingDefault = false;
        private const bool ShowQuantitySettingDefault = false;
        private const bool ShowPrioritySettingDefault = false;

        private const bool HideCheckedItemsSettingDefault = false;
        private const bool FastScrollingSettingDefault = false;
        private const bool ShakeToCleanUpSettingDefault = false;
        private const bool TrackPerStorePricesSettingDefault = false;
        private const bool DisableScreenLockSettingDefault = false;
        private const bool QuickModeeditSettingDefault = false;
        private const bool FiltersSettingDefault = false;
        private const bool ResetQuantitySettingDefault = false;
        //End of default Application Settings

        public int FontSize { get; set; }
        public int SortOrder { get; set; }

        public bool ShowPrice { get; set; }
        public bool ShowTags { get; set; }
        public bool ShowUnits { get; set; }
        public bool ShowQuantity { get; set; }
        public bool ShowPriority { get; set; }

        public bool HideCheckedItems { get; set; }
        public bool FastScrolling { get; set; }
        public bool ShakeToCleanUp { get; set; }
        public bool TrackPerStorePrices { get; set; }
        public bool DisableScreenLock { get; set; }
        public bool QuickEditMode { get; set; }
        public bool Filters { get; set; }
        public bool ResetQuality { get; set; }

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

        /// <summary>
        /// Property to get and set a Font Size Setting Key.
        /// </summary>
        public int SelectedListIndexSetting
        {
            get
            {
                return GetValueOrDefault<int>(SelectedListIndexSettingKeyName, SelectedListIndexSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(SelectedListIndexSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Font Size Setting Key.
        /// </summary>
        public int FontSizeSetting
        {
            get
            {
                return GetValueOrDefault<int>(FontSizeSettingKeyName, FontSizeSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(FontSizeSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Font Size Setting Key.
        /// </summary>
        public bool ShowTagsSettings
        {
            get
            {
                return GetValueOrDefault<bool>(ShowTagsSettingKeyName, ShowTagsSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(ShowTagsSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Font Size Setting Key.
        /// </summary>
        public bool FiltersSettings
        {
            get
            {
                return GetValueOrDefault<bool>(FiltersSettingKeyName, FiltersSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(FiltersSettingKeyName, value))
                {
                    Save();
                }
            }
        }
    }
}
