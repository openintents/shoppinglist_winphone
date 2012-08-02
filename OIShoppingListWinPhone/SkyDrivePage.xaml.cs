using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using Microsoft.Live;

using OIShoppingListWinPhone.DataModel;

namespace OIShoppingListWinPhone
{
    public partial class SkyDrivePage : PhoneApplicationPage
    {
        private LiveConnectSession LiveSession;
        private LiveConnectClient client;

        private string fileBody;
        private string fileName;

        public SkyDrivePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ListID"))
            {
                int listId = Convert.ToInt32(NavigationContext.QueryString["ListID"]);
                ShoppingList currentList = App.ViewModel.ShoppingLists.FirstOrDefault(x => x.ListID == listId);
                fileName = currentList.ListName;
                if (currentList != null)
                {
                    foreach (ShoppingListItem item in currentList.ListItems)
                    {
                        fileBody += String.Format("[{0}],", item.Status);
                        fileBody += item.Quantity == null ? "," : item.Quantity + ",";
                        fileBody += item.Units == null ? "," : item.Units + ",";
                        fileBody += String.Format("{0},", item.ItemName);
                        fileBody += item.Tag == string.Empty ? "," : item.Tag + ",";
                        fileBody += item.Price == 0.00F ? "" : String.Format("{0:F2}", item.Price);
                        fileBody += "\n\n\n\n";
                    }
                }
            }
        }

        private void signInButton1_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == LiveConnectSessionStatus.Connected)
            {
                client = new LiveConnectClient(e.Session);
                LiveSession = e.Session;
                textBlock1.Text = "You are signed in!";
                button1.IsEnabled = true;
            }
            else
            {
                client = null;
            }
        }
                
        private void button1_Transmit(object sender, RoutedEventArgs e)
        {
            LiveConnectClient client = new LiveConnectClient(LiveSession);
            client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetCompleted);

            ProgressIndicator prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = true;
            prog.Text = "File uploading...";
            SystemTray.SetProgressIndicator(this, prog);

            client.GetAsync("me/skydrive/files");            
        }

        void client_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string folder_id = string.Empty;
                List<object> folder_data = (List<object>)e.Result["data"];
                foreach (object data in folder_data)
                {
                    IDictionary<string, object> data_set = (IDictionary<string, object>)data;
                    if (data_set["name"].ToString() == "OIShoppingList for Windows Phone 7"
                        && data_set["type"].ToString() == "folder")
                        folder_id = data_set["id"].ToString();
                }
                if (folder_id == string.Empty)
                    folder_id = "/me/skydrive";

                byte[] byteArray = Encoding.Unicode.GetBytes(fileBody);
                MemoryStream fileStream = new MemoryStream(byteArray);

                LiveConnectClient uploadClient = new LiveConnectClient(LiveSession);
                uploadClient.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(uploadClient_UploadCompleted);
                uploadClient.UploadAsync(folder_id, fileName + ".txt", fileStream);
            }
            else
            {
                MessageBox.Show("There is an error occur during file uploading: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                SystemTray.ProgressIndicator.IsVisible = false;
            }
        }
        
        void uploadClient_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            if (e.Error == null)
            {
                MessageBox.Show("Your data have been successfully saved to the SkyDrive", "All Done!", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("There is an error occur during file uploading: " + e.Error.Message, "Warning", MessageBoxButton.OK);
            }            
        }
    }
}