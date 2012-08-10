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
using Microsoft.Live;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace OIShoppingListWinPhone
{
    public partial class SkyDriveItemPage : PhoneApplicationPage
    {
        //Live Connect Session handler
        private LiveConnectSession LiveSession;

        //Live Connect Client handler
        private LiveConnectClient client;

        //String file ID
        private string FileID = string.Empty;

        //File name
        private string FileName = string.Empty;

        //File created date
        private DateTime CreatedDate;

        //Name of the user who uploaded the file
        private string UserName = string.Empty;

        //A value that indicates whether comments are enabled for the file.
        //If comments can be made, this value is true; otherwise, it is false
        private bool CommentsEnabled = false;

        //Private field for saving collection of SkyDrive files
        private ObservableCollection<SkyDriveComment> SkyDriveCommentsCollection
            = new ObservableCollection<SkyDriveComment>();

        //String shared READ link
        private string Shared_Read_Link = string.Empty;

        //String shared EDIT link
        private string Shared_Edit_Link = string.Empty;

        public SkyDriveItemPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SkyDriveItemPage_Loaded);
        }

        //OnNavigatedTo method using for geting FileID and starting to download file information
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Checking whether QueryString contain 'FileID' key
            if (NavigationContext.QueryString.ContainsKey("FileID"))
            {
                //Get all file properties
                FileID = NavigationContext.QueryString["FileID"].ToString();
                FileName = NavigationContext.QueryString["Name"].ToString();
                DateTime.TryParse(NavigationContext.QueryString["CreatedDate"].ToString(), out CreatedDate);
                UserName = NavigationContext.QueryString["UserName"];
                bool.TryParse(NavigationContext.QueryString["CommentsEnabled"], out CommentsEnabled);                
            }
        }
        
        //SkyDriveItemPage Loaded event hamdler
        void SkyDriveItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialize LiveConnectSession with Loaded page event
            this.LiveSession = Application.Current.Resources["LiveSession"] as LiveConnectSession;
            if (this.LiveSession != null)
                //Initialising client with current session
                client = new LiveConnectClient(LiveSession);                
            else
            {
                MessageBox.Show("There is an error occur during loading the page", "Warning", MessageBoxButton.OK);
                NavigationService.GoBack();
            }

            //Update UI
            this.FileNameTextBlock.Text = this.FileName;
            this.UserNameTextBlock.Text = this.UserName;
            this.CreatedDateTextBlock.Text = this.CreatedDate.ToString();

            if (this.CommentsEnabled)
                this.LoadComments();
        }

        //Loading comments for corresponding file
        private void LoadComments()
        {
            this.SkyDriveCommentsCollection.Clear();
            CommentsLoadingProgressBar.Visibility = System.Windows.Visibility.Visible;
            AddNewCommentPanel.Visibility = System.Windows.Visibility.Visible;
            //Start loading comments
            client = new LiveConnectClient(LiveSession);
            client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetCommentsCompleted);
            client.GetAsync(FileID + "/comments");
        }

        void client_GetCommentsCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //Updating Comments ListBox ItemsSource
                List<object> data = (List<object>)e.Result["data"];
                foreach (IDictionary<string, object> datum in data)
                {
                    //Create SkyDriveComment object and add it to
                    //Comments ListBox ItemsSource collection
                    SkyDriveComment comment = new SkyDriveComment();
                    comment.Message = datum["message"].ToString();
                    DateTime date;
                    DateTime.TryParse(datum["created_time"].ToString(), out date);
                    comment.CreatedDate = date;

                    comment.CommentID = datum["id"].ToString();

                    IDictionary<string, object> user_data = datum["from"] as IDictionary<string, object>;
                    comment.UserName = user_data["name"].ToString();

                    SkyDriveCommentsCollection.Add(comment);
                }
                CommentListBox.ItemsSource = this.SkyDriveCommentsCollection;
                CommentsLoadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
        }      

        private void AddNewComment_Click(object sender, RoutedEventArgs e)
        {
            if (NewCommentTextBox.Text.Trim() != string.Empty)
            {
                Dictionary<string, object> comment = new Dictionary<string, object>();
                comment.Add("message", NewCommentTextBox.Text);
                client.PostCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_PostCommentCompleted);
                client.PostAsync(FileID + "/comments", comment);
                NewCommentTextBox.Text = "";                
            }
            else
                MessageBox.Show("Please, input comment!", "Information", MessageBoxButton.OK);
        }

        void client_PostCommentCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
                this.LoadComments();
            else
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
        }

        private void DeleteCommentButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj);

            if (obj != null)
            {
                SkyDriveComment comment = (obj as ListBoxItem).DataContext as SkyDriveComment;
                client.DeleteCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_DeleteCommentCompleted);
                client.DeleteAsync(comment.CommentID);
            }
        }

        void client_DeleteCommentCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
                this.LoadComments();
            else
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
        }

        private void ShareReadLink_Click(object sender, RoutedEventArgs e)
        {
            ProgressIndicator prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.Text = "Loading...";
            prog.IsVisible = true;
            SystemTray.ProgressIndicator = prog;
            //Get shared_read_link for current file
            client = new LiveConnectClient(LiveSession);
            client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetFileSharedReadLinkCompleted);
            client.GetAsync(FileID + "/shared_read_link");
        }

        void client_GetFileSharedReadLinkCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                if (!this.CommentsEnabled)
                {
                    this.CommentsEnabled = true;
                    AddNewCommentPanel.Visibility = System.Windows.Visibility.Visible;
                }
                Shared_Read_Link = e.Result["link"].ToString();

                EmailComposeTask email = new EmailComposeTask();
                email.Body = "Here is link to my '" + FileName + "' shopping list.\n";
                email.Body += Shared_Read_Link;
                email.Body += "\nYou can read this file.";
                email.Show();
            }
            else
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
        }

        private void ShareEditLink_Click(object sender, RoutedEventArgs e)
        {
            ProgressIndicator prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.Text = "Loading...";
            prog.IsVisible = true;
            SystemTray.ProgressIndicator = prog;
            //Get shared_edit_link for current file
            client = new LiveConnectClient(LiveSession);
            client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(client_GetFileSharedEditLinkCompleted);
            client.GetAsync(FileID + "/shared_read_link");
        }

        void client_GetFileSharedEditLinkCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                if (!this.CommentsEnabled)
                {
                    this.CommentsEnabled = true;
                    AddNewCommentPanel.Visibility = System.Windows.Visibility.Visible;
                }
                Shared_Edit_Link = e.Result["link"].ToString();
                
                EmailComposeTask email = new EmailComposeTask();
                email.Body = "Here is link to my '" + FileName + "' shopping list.\n";
                email.Body += Shared_Edit_Link;
                email.Body += "\nYou can read and edit this file.";
                email.Show();
            }
            else
                MessageBox.Show("There is an error occur during loading data from SkyDrive: " + e.Error.Message, "Warning", MessageBoxButton.OK);
        }        
    }

    /// <summary>
    /// Class for save SkyDrive files info in useful way.
    /// Object collection of this class will be used for SkyDriveDataListBox ItemsSource
    /// </summary>
    public sealed class SkyDriveComment : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// Comment unique ID
        /// </summary>
        public string CommentID { get; set; }

        /// <summary>
        /// The name of the user who created the comment
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The text of the comment. The maximum length of a comment is 10,000 characters.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The time at which the comment was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        public SkyDriveComment() { }

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