using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibraryManagementSystem
{
    // Abstract Class
    public abstract class BookItem
    {
        private string? isbn, title, author, publisher, edition;
        private int quantity, availQuantity, yearOfPublication;

        //Book APA Format
        public string? ISBN { get => isbn; set => isbn = value; }
        public string? Title { get => title; set => title = value; }
        public string? Author { get => author; set => author = value; }
        public int YearOfPublication { get => yearOfPublication; set => yearOfPublication = value; }
        public string? Publisher { get => publisher; set => publisher = value; }
        public string? Edition { get => edition; set => edition = value; }

        //Stats
        public int Quantity { get => quantity; set => quantity = value; }
        public int AvailableQuantity { get => availQuantity; set => availQuantity = value; }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            library.RunLibraryMenu();
        }
    }
}