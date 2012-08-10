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
using System.ComponentModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using Microsoft.Live;
using System.Collections.ObjectModel;

using OIShoppingListWinPhone.DataModel;

namespace OIShoppingListWinPhone
{
    public partial class SkyDrivePage : PhoneApplicationPage
    {
        //Live Connect Session handler
        private LiveConnectSession LiveSession;
        //Live Connect Client handler
        private LiveConnectClient client;

        //SkyDrive ID of 'OI Shopping List' folder
        private string folder_id = string.Empty;
        //String body of uploading file
        private string fileBody = string.Empty;
        //String file name of uploading file
        private string fileName = string.Empty;
        //Private field for saving collection of SkyDrive files
        private ObservableCollection<SkyDriveFileInfo> SkyDriveFilesCollection
            = new ObservableCollection<SkyDriveFileInfo>();

        public SkyDrivePage()
        {
            InitializeComponent();
        }

        //OnNavigatedTo method using for initialising uploading file name and file body
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ListID"))
            {
                int listId = Convert.ToInt32(NavigationContext.QueryString["ListID"]);
                //Get ShoppingList wuth corresponding list ID
                ShoppingList currentList = App.ViewModel.ShoppingLists.FirstOrDefault(x => x.ListID == listId);
                //Get uploading file name
                fileName = currentList.ListName;
                FileName.Text = fileName + ".txt";

                //Get uploading file body
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
                        fileBody += "\n";
                    }
                }
            }
        }        

        //SessionChanged event - fires wneh the status of LiveConnectSession was changed
        private void signInButton1_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == LiveConnectSessionStatus.Connected)
            {
                //Create new client if LiveConnectSessionStatus is Connected
                client = new LiveConnectClient(e.Session);
                LiveSession = e.Session;
                //Save LiveSession in Application.Current.Resources
                //for using in SkyDriveItem page
                if (!Application.Current.Resources.Contains("LiveSession"))
                    Application.Current.Resources.Add("LiveSession", LiveSession);
                else
                {
                    Application.Current.Resources.Remove("LiveSession");
                    Application.Current.Resources.Add("LiveSession", LiveSession);
                }
                //Indicating, than user is signed in
                textBlock1.Text = "You are signed in!";
                //Clear SkyDriveListBox items (in the case, if user click 'SignOut')
                SkyDriveFilesCollection.Clear();

                //Set SystemTray.ProgressIndicator
                ProgressIndicator prog = new ProgressIndicator();
                prog.IsIndeterminate = true;
                prog.IsVisible = true;
                prog.Text = "Loading...";
                SystemTray.SetProgressIndicator(this, prog);

                //Begin initialisation of data transmition
                //Loading 'OI Shopping List' folder ID
                client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetRootDirectoryEnrtiesList);
                client.GetAsync("me/skydrive/files");
            }
            else
            {
                client = null;
                ListBoxLabel.Opacity = 0.5;
                SkyDriveDataListBox.IsEnabled = false;
                UploadButton.IsEnabled = false;
            }
        }
        
        //Get root SkyDrive directory entries list Completed
        void client_GetRootDirectoryEnrtiesList(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                folder_id = this.GetFolderId(e);
                //If there is NO 'OI Shopping List' folder -> create new
                if (folder_id == string.Empty)
                {
                    Dictionary<string, object> folderData = new Dictionary<string, object>();
                    folderData.Add("name", "OI Shopping List");
                    LiveConnectClient createFolderClient = new LiveConnectClient(LiveSession);
                    createFolderClient.PostCompleted += new EventHandler<LiveOperationCompletedEventArgs>(createFolderClient_PostCompleted);
                    createFolderClient.PostAsync("/me/skydrive", folderData);
                }
                else
                {
                    //Set SystemTray.ProgressIndicator.Visibility = false
                    //and enabling file upload button after receiving 'OI Shopping List' folder ID
                    SystemTray.ProgressIndicator.IsVisible = false;
                    UploadButton.IsEnabled = true;
                    //Start downloading SkyDrive data list
                    this.DownloadSkyDriveDataList();
                }
                
            }
            else
            {
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                SystemTray.ProgressIndicator.IsVisible = false;
                NavigationService.GoBack();
            }
        }

        //Finish create 'OI Shopping List' folder in SkyDrive storage
        void createFolderClient_PostCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            //If folder was created and there is no errors -> get folder id and 
            //than enable button for uloading the file to 'OI Shopping List' folder
            if (e.Error == null)
            {
                //Begin initialisation of data transmition
                //Loading 'OI Shopping List' folder ID
                LiveConnectClient client = new LiveConnectClient(LiveSession);
                client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetRootDirectoryEnrtiesList);
                client.GetAsync("me/skydrive/files");                
            }
            else
            {
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                SystemTray.ProgressIndicator.IsVisible = false;
                NavigationService.GoBack();
            }
        }

        /// <summary>
        /// Get 'OI Shopping List' folder ID
        /// </summary>
        /// <param name="e">LiveOperationCompletedEventArgs with 'me/skydrive' folder data</param>
        /// <returns>'OI Shopping List' ID if exist, else - null</returns>
        private string GetFolderId(LiveOperationCompletedEventArgs e)
        {
            //Enumeration all '/me/skedrive' folder entries
            //and get 'OI Shopping List' folder ID if it's exist
            string folderid = string.Empty;
            List<object> folder_data = (List<object>)e.Result["data"];
            foreach (object data in folder_data)
            {
                IDictionary<string, object> data_set = (IDictionary<string, object>)data;
                if (data_set["name"].ToString() == "OI Shopping List"
                    && data_set["type"].ToString() == "folder")
                    folderid = data_set["id"].ToString();
            }
            return folderid;
        }

        //Upload button click event handler
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            //Call method for uploading file to SkyDrive
            this.UploadFile();
        }

        private void UploadFile()
        {
            //Creating byte array from 'fileBody' string
            byte[] byteArray = Encoding.Unicode.GetBytes(fileBody);
            MemoryStream fileStream = new MemoryStream(byteArray);

            //Set SystemTray.ProgressIndicator
            ProgressIndicator prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = true;
            prog.Text = "File uploading...";
            SystemTray.SetProgressIndicator(this, prog);

            //Uploading the file to SkyDrive storage
            LiveConnectClient uploadClient = new LiveConnectClient(LiveSession);
            uploadClient.UploadCompleted += new EventHandler<LiveOperationCompletedEventArgs>(uploadClient_UploadCompleted);
            uploadClient.UploadAsync(folder_id, fileName + ".txt", fileStream);
        }
        
        //Finish uploading the file to SkyDrive storage
        void uploadClient_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("'"+ fileName + "' have been successfully saved to the SkyDrive", "All Done!", MessageBoxButton.OK);
                SystemTray.ProgressIndicator.IsVisible = false;
                //Maring 'invisible' UI upload file section
                ((FrameworkElement)ContentPanel.Children[1]).Height = 0;
                //Update SkyDrive data list
                this.DownloadSkyDriveDataList();
            }
            else
            {
                if (e.Error.Message == "The resource could not be created. The resource '"
                    + fileName + ".txt" + "' already exists.")
                {
                    if (MessageBox.Show("File with this name is already exist in SkyDrive data storage\nDo you want to rewrite existing file?",
                        "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //If 'Rewrite existing file?' message box result == OK -> delete existing file
                        //and upload new
                        //At first it's needed to get existing file ID
                        LiveConnectClient client = new LiveConnectClient(LiveSession);
                        //client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetDataFolderEntriesCompleted);
                        //client.GetAsync(folder_id + "/files");
                    }
                    else
                    {
                        //If 'Rewrite existing file?' message box result == Cancel -> stop uploading file.
                        //In this case user will be able to rename existing 
                    }
                }
                else
                {
                    MessageBox.Show("There is an error occur during file uploading: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
            }
        }

        /*void client_GetDataFolderEntriesCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string exFileID = string.Empty;
                exFileID = this.GetUploadedFileID(fileName + ".txt", e);
                if (exFileID != string.Empty)
                {
                    //Deleting existing file
                    LiveConnectClient delCilent = new LiveConnectClient(LiveSession);
                }
                else
                {
                }
            }
            else
            {
                MessageBox.Show("There is an error occur during file uploading: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                SystemTray.ProgressIndicator.IsVisible = false;
            }
        }*/

        /// <summary>
        /// Get unique of uloaded file
        /// </summary>
        /// <param name="existingFileName">String name of uploaded file</param>
        /// <param name="e">Live operation completed event argument</param>
        /// <returns>String uploaded file ID</returns>
        private string GetUploadedFileID(string existingFileName, LiveOperationCompletedEventArgs e)
        {
            string existingFileID = string.Empty;

            //Enumeration all 'OI Shopping List' folder files
            //and get existing file ID (where - exesting_file_name = uploading_file_name)
            List<object> folder_data = (List<object>)e.Result["data"];
            foreach (object data in folder_data)
            {
                IDictionary<string, object> data_set = (IDictionary<string, object>)data;
                if (data_set["name"].ToString() == existingFileName + ".txt"
                    && data_set["type"].ToString() == "file")
                    existingFileID = data_set["id"].ToString();
            }

            return existingFileID;
        }

        //Uploading all SkyDrive saved lists from the 'OI Shopping List' folder
        //and than Updating SkyDrive data ListBox ItemsSource
        private void DownloadSkyDriveDataList()
        {
            if (folder_id != string.Empty)
            {
                this.SkyDriveFilesCollection.Clear();
                ListBoxLoadingProgressBar.Visibility = System.Windows.Visibility.Visible;

                LiveConnectClient client = new LiveConnectClient(LiveSession);
                client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetDataFolderEntriesCompleted);
                client.GetAsync(folder_id + "/files");
            }
            else
            {
                MessageBox.Show("There is an error occur during loading data from SkyDrive", "Warning", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
        }

        //Get 'OI Shopping List' folder properties
        void client_GetDataFolderEntriesCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //Set ItemsSource collection for SkyDriveListBox
                SkyDriveDataListBox.ItemsSource = this.SkyDriveFilesCollection;
                SkyDriveDataListBox.IsEnabled = true;
                List<object> folder_data = (List<object>)e.Result["data"];
                if (folder_data.Count == 0)
                {
                    //There are NO files in 'OI Shopping List' folder
                    ListBoxLoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    //Enumeration all 'OI Shopping List' folder files
                    //to get it's properties
                    foreach (object data in folder_data)
                    {
                        IDictionary<string, object> data_set = (IDictionary<string, object>)data;
                        if (data_set["type"].ToString() == "file")
                        {
                            //Get file properties of all existing in 'OI Shopping List' folder
                            LiveConnectClient client = new LiveConnectClient(LiveSession);
                            client.GetCompleted +=
                                new EventHandler<LiveOperationCompletedEventArgs>(client_GetFileInformationDownloadCompleted);
                            client.GetAsync(data_set["id"].ToString());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                NavigationService.GoBack();
            }                                   
        }

        //Get properties of file existing in 'OI Shopping List' folder
        void client_GetFileInformationDownloadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //Get SkyDriveFileInfo from e.Result
                IDictionary<string, object> fileProperties = e.Result;
                SkyDriveFileInfo fileInfo = new SkyDriveFileInfo();

                //Make string without '.txt' in the end of file name
                string str = fileProperties["name"].ToString();
                fileInfo.Name = str.Substring(0, str.Length - 4);

                //Parsing file date time
                DateTime fileInfoCreatedTime = new DateTime();
                DateTime.TryParse(fileProperties["created_time"].ToString(), out fileInfoCreatedTime);
                fileInfo.CreatedDate = fileInfoCreatedTime;

                //Save file info properties
                fileInfo.FileID = fileProperties["id"].ToString();
                fileInfo.CommentEnabled = (bool)fileProperties["comments_enabled"];

                IDictionary<string, object> user_data = (IDictionary<string, object>)fileProperties["from"];
                fileInfo.UserName = user_data["name"].ToString();

                //Add new SkyDriveFileInfo to ListBox collection
                this.SkyDriveFilesCollection.Add(fileInfo);
                
                ListBoxLabel.Opacity = 1.0;
                ListBoxLoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
        }
        
        private void RenameMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            DependencyObject obj = menu.Tag as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj);

            if (obj != null)
            {
                SkyDriveFileInfo info = SkyDriveDataListBox.SelectedItem as SkyDriveFileInfo;
            }
        }

        private void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        //SkyDriveDataListBox.Item.Grid Taped event handler.
        //When this event fires -> navigate to SkyDriveItemPage
        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj);

            if (obj != null)
            {
                //Get SkyDriveFileInfo as selected ListBoxItem DataContext
                SkyDriveFileInfo fileInfo = (obj as ListBoxItem).DataContext as SkyDriveFileInfo;
                //Creating query body
                string queryBody = string.Empty;
                queryBody += "?FileID=" + fileInfo.FileID;
                queryBody += "&Name=" + fileInfo.Name;
                queryBody += "&CreatedDate=" + fileInfo.CreatedDate;
                queryBody += "&UserName=" + fileInfo.UserName;
                queryBody += "&CommentsEnabled=" + fileInfo.CommentEnabled;

                //Navigation to SkyDriveItem page
                NavigationService.Navigate(new Uri("/SKyDriveItemPage.xaml" + queryBody, UriKind.Relative));
            }
        }

        //Download button click event handler
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        
    }

    /// <summary>
    /// Class for save SkyDrive files info in useful way.
    /// Object collection of this class will be used for SkyDriveDataListBox ItemsSource
    /// </summary>
    public sealed class SkyDriveFileInfo : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// File unique id
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// The name of the user who uploaded the file
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// A value that indicates whether comments are enabled for the file.
        /// If comments can be made, this value is true; otherwise, it is false
        /// </summary>
        public bool CommentEnabled { get; set; }

        public SkyDriveFileInfo() { }

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