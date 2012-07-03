﻿using System;
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

namespace OIShoppingListWinPhone
{
    public sealed class Settings
    {
        public int FontSize { get; set; }
        public int SortOrder { get; set; }

        public enum FontSizeSettings
        {
            Tiny = 20,
            Small = 30,
            Default = 45,
            Large = 64
        }

        public enum SortOrderSettings
        {
            UncheckedFirst_Alphabetical = 0,
            Alphabetical = 1,
            NewestFirst = 2,
            OldestFirst = 3,
            TagsAlphabetical = 4,
            Priority_TagsAlphabetical = 5,
            MostExpensiveFirst = 6,
            UncheckedFirst_TagsAlphabetic = 7,
            UncheckedFirst_Priority_Alphabetical = 8,
            UnckeckedFirst_Priority_TagsAlphabetical = 9
        }

        public Settings()
        {
            FontSize = (int)FontSizeSettings.Default;
            SortOrder = (int)SortOrderSettings.OldestFirst;
        }

        public void Save()
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = iso.CreateFile("OIShoppingListSettings.xml");
            StreamWriter writer = new StreamWriter(stream);

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(writer, this);

            writer.Close();
            iso.Dispose();
        }

        public static Settings Load()
        {
            Settings settings = new Settings();

            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            if (iso.FileExists("OIShoppingListSettings.xml"))
            {
                IsolatedStorageFileStream stream = iso.OpenFile("OIShoppingListSettings.xml", FileMode.Open);
                StreamReader reader = new StreamReader(stream);

                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                settings = serializer.Deserialize(reader) as Settings;

                reader.Close();
            }
            else
            {
                settings = new Settings();
            }

            return settings;
        }
    }
}
