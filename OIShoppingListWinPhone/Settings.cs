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

namespace OIShoppingListWinPhone
{
    public sealed class Settings
    {
        public void Save()
        {
        }

        public static Settings Load()
        {
            Settings settings = new Settings();
            return settings;
        }
    }
}
