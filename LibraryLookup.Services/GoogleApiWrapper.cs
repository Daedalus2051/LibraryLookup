using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace LibraryLookup.Services
{
    public class GoogleApiWrapper
    {
        private UserCredential apiCredential;
        private BooksService booksService;

        public GoogleApiWrapper()
        {
            IsAuthenticated = false;
        }

        public async void Authenticate(string secretsFile)
        {
            await AuthenticateGoogleApi(new FileStream(secretsFile, FileMode.Open));
        }
        public async void Authenticate(Stream secretsStream)
        {
            await AuthenticateGoogleApi(secretsStream);
        }
        public bool IsAuthenticated { get; private set; }

        private async Task AuthenticateGoogleApi(Stream secretStream)
        {
            try
            {
                apiCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(secretStream).Secrets,
                    new[] { BooksService.Scope.Books },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary")
                );
                booksService = new BooksService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = apiCredential,
                    ApplicationName = "Library Lookup"
                });

                IsAuthenticated = true;
            }
            catch (Exception ex)
            {
                //TODO: Add logging so we know what happened
                Console.WriteLine($"Error occurred during authentication: {ex.Message}");
                IsAuthenticated = false;
            }
        }

        public Volumes SearchBooks(string searchCriteria)
        {
            Volumes volumes = new Volumes();
            try
            {
                VolumesResource volumesResource = new VolumesResource(booksService);
                volumes = volumesResource.List(searchCriteria).Execute();
            }
            catch (Exception ex)
            {
                //TODO: Add logging
                Console.WriteLine($"An error occurred getting volumes: {ex.Message}");
            }
            return volumes;
        }
    }
}
