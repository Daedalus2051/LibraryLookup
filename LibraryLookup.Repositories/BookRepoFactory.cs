using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLookup.Repositories
{
    public class BookRepoFactory
    {
        public BookRepoFactory()
        {

        }

        public IBookRepository GetBookRepository(string repoType)
        {
            switch(repoType.ToLower())
            {
                case "google":
                    return new GoogleBooksRepository();
                    
                default:
                    return null;
            }
        }
    }
}
