using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace LibraryLookup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected string google_booksOauthUri = @"https://www.googleapis.com/auth/books";
        internal UserCredential credential;
        internal BooksService _booksService;

        public MainWindow()
        {
            InitializeComponent();

            AuthenticateGoogleApi();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            VolumesResource volumesResource = new VolumesResource(_booksService);
            var volumeList = volumesResource.List(TxtInput.Text).Execute();

            string volumeResults = "";
            foreach(var item in volumeList.Items)
            {
                volumeResults += $"{item.VolumeInfo.Title}{Environment.NewLine}";
            }

            TxtblkDisplay.Text = volumeResults;
        }
        
        private async Task AuthenticateGoogleApi()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LibraryLookup.client_secrets.json")) //new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { BooksService.Scope.Books },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            }

            // Create the service.
            _booksService = new BooksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Library Lookup",
            });
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                //MessageBox.Show("Tab was pressed!");
                // TODO: Auto search for the ISBN/title/search that was entered
            }
        }
    }
}
