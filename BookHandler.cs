﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class BookHandler : BookItem
    {
        //Validation
        public void ValidateAddBook(string isbn, string title, string author,int yearOfPublication, string publisher, string edition, int quantity)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(publisher) || string.IsNullOrWhiteSpace(edition) ||
                yearOfPublication < 0 || quantity < 0)
            {
                FileHandler.ShowMessageWithDelay("Error: Title, Author, Publisher, Edition, and Year of Publication cannot be blank or invalid.", ConsoleColor.Red);
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                return;
            }

            FileHandler.AddBook(title, author, yearOfPublication, publisher, edition, quantity);
            FileHandler.ShowMessageWithDelay("Book added.", ConsoleColor.Green);
        }
    }
}