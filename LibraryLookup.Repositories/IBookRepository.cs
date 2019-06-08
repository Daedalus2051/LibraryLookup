using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLookup.Repositories
{
    public interface IBookRepository
    {
        IEnumerable GetBooks(string search);
        object GetBook(string bookId);
    }
}
