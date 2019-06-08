using LibraryLookup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLookup.Repositories
{
    public class GoogleBooksRepository : IBookRepository
    {
        private GoogleApiWrapper googleApi;

        public GoogleBooksRepository()
        {
            googleApi = new GoogleApiWrapper();
        }

        public object GetBook(string bookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetBooks(string search)
        {
            if (!googleApi.IsAuthenticated)
            {
                googleApi.Authenticate(Assembly.GetExecutingAssembly().GetManifestResourceStream("LibraryLookup.client_secret.json"));
            }

            googleApi.SearchBooks(search);
        }
    }
}
