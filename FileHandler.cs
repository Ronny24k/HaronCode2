﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public static class FileHandler
    {
        private static string booksFile = @"C:\LibraryData\books.txt";
        private static string usersFile = @"C:\LibraryData\users.txt";
        private static string folderFile = @"C:\LibraryData\";

        private static string GenerateUniqueISBN()
        {
            return $"{DateTime.Now.Ticks % 10000000:D7}";
        }

        private static void CheckFolder()
        {
            if (!Directory.Exists(folderFile))
                Directory.CreateDirectory(folderFile);
            else
                return;
        }

        public static void ShowMessageWithDelay(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        public static string Login(string UserId, string password)
        {
            CheckFolder();
            if (!File.Exists(usersFile)) 
                return null;

            string[]? lines = null;

            try
            {
                lines = File.ReadAllLines(usersFile); // Read all lines to allow modifications
            }
            catch (IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file is currently in use. Please try again later.");
                Console.ResetColor();
                return null;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split('|');

                if (parts[0] == UserId && Authentication(password, parts[2]))
                {
                    return parts[3];
                }
            }
            return null;
        }

        //Verify the password
        private static bool Authentication(string inputPassword, string storedPassword)
        {
            string Input = inputPassword;
            return Input == storedPassword;
        }

        public static void AddUser(string userId, string name, string password, string role) //counted as register methods
        {
            CheckFolder();
            if (!File.Exists(usersFile))
                File.Create(usersFile);

            // Validate inputs
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: All fields (User ID, Name, Password, Role) are required.");
                Console.ResetColor();
                return;
            }

            string roleHolder = role.ToLower(); //This lowers the capitalization of the role regardless of the input 
            try
            {
                using (StreamWriter sw = new StreamWriter(usersFile, append: true))
                {
                    sw.WriteLine($"{userId}|{name}|{password}|{roleHolder}");
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured: " + ex.Message);
            }

            ShowMessageWithDelay($"User '{name}' added successfully.");
        }

        public static void DeleteUser(string userId)
        {
            CheckFolder();

            if (!File.Exists(usersFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Users file does not exist.");
                Console.ResetColor();
                return;
            }

            string borrowFile = @"C:\LibraryData\borrowedBooks.txt";
            if (File.Exists(borrowFile))
            {
                // Check if the user has borrowed any books
                try
                {
                    using (StreamReader reader = new StreamReader(borrowFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 3 && parts[0] == userId)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error: User ID {userId} cannot be deleted because they have borrowed books.");
                                Console.ResetColor();
                                return; // Exit the method if borrowed books are found
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error reading borrowed books file: {ex.Message}");
                    Console.ResetColor();
                    return;
                }
            }

            // Proceed with user deletion if no borrowed books are found
            string tempFile = Path.GetTempFileName();
            bool userFound = false;

            try
            {
                using (StreamReader reader = new StreamReader(usersFile))
                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts[0] == userId)
                        {
                            userFound = true;
                            continue; // Skip writing this user's line to the temp file
                        }

                        writer.WriteLine(line);
                    }
                }

                if (userFound)
                {
                    File.Delete(usersFile);
                    File.Move(tempFile, usersFile);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("User successfully deleted!");
                    Console.ResetColor();
                }
                else
                {
                    File.Delete(tempFile);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"User ID {userId} was not found.");
                    Console.ResetColor();
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        //List
        public static List<string> LoadUsers()
        {
            List<string> users = new List<string>();

            if (!File.Exists(usersFile))
            {
                Console.WriteLine("User file does not exist. No users to load.");
                return users;
            }

            try
            {
                using(StreamReader reader = new StreamReader(usersFile))
                {
                    string? line;
                    while((line = reader.ReadLine()) != null)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Split the line into parts
                        string[] parts = line.Split('|');

                        if (parts.Length >= 4) // Ensure the line has all required fields
                        {
                            string userInfo = $"UserID: {parts[0]}, Name: {parts[1]}, Role: {parts[3]}";

                            // Add the user to the list
                            users.Add(userInfo);

                        }
                        else
                        {
                            Console.WriteLine($"Warning: Skipping malformed line: {line}");
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error loading users: {ex.Message}"); Console.ResetColor();
            }

            return users;
        }

        public static List<string> LoadAllBooks()
        {
            List<string> loadedBooks = new List<string>();

            if (!File.Exists(usersFile))
            {
                Console.WriteLine("User file does not exist. No users to load.");
                return loadedBooks;
            }

            try
            {
                using (StreamReader reader = new StreamReader(usersFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Split the line into parts
                        string[] parts = line.Split('|');

                        if (parts.Length >= 7) // Ensure the line has all required fields
                        {
                            string bookInfo = $"BookID: {parts[0]} | Title: {parts[1]} | Author: {parts[2]} | Publisher: {parts[3]} | Edition: {parts[4]} | " +
                                              $"Year of Publication: {parts[5]} | Quantity: {parts[6]}";

                            // Add the book to the list
                            loadedBooks.Add(bookInfo);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: Skipping malformed line: {line}");
                        }
                    }
                }

                // Sort books alphabetically by Title using a custom comparer
                loadedBooks.Sort(new BookTitleComparer());
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error loading books: {ex.Message}");
                Console.ResetColor();
            }

            return loadedBooks;
        }

        private class BookTitleComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                // Extract titles from book strings
                string titleX = ExtractTitle(x);
                string titleY = ExtractTitle(y);

                // Compare titles
                return string.Compare(titleX, titleY, StringComparison.OrdinalIgnoreCase);
            }

            private string ExtractTitle(string bookInfo)
            {
                // Extract the Title field assuming format: "BookID: {ID} | Title: {Title} | ..."
                string[] parts = bookInfo.Split('|');
                foreach (var part in parts)
                {
                    if (part.Trim().StartsWith("Title:", StringComparison.OrdinalIgnoreCase))
                    {
                        return part.Split(':')[1].Trim();
                    }
                }
                return string.Empty; // Default to an empty string if Title is not found
            }
        }

        //Book File Handling
        public static void AddBook(string title, string author, int yearOfPublication, string
            publisher, string edition, int quantity)


        {
            var isbnTemp = GenerateUniqueISBN();
            try
            {
                using (StreamWriter writer = new StreamWriter(booksFile, append: true))
                {
                    writer.WriteLine($"{isbnTemp}|{title}|{author}|{publisher}|{edition}|{yearOfPublication}|{quantity}");
                }
                ShowMessageWithDelay("Book added successfully!", ConsoleColor.Green);
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error adding book: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void DeleteBook(string bookTitle)
        {
            CheckFolder();

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return;
            }

            List<string> updatedBooks = new List<string>();
            bool bookFound = false;

            try
            {
                using (StreamReader reader = new StreamReader(booksFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');

                        if (parts.Length >= 7 && parts[1].Equals(bookTitle, StringComparison.OrdinalIgnoreCase))
                        {
                            bookFound = true;
                            continue; // Skip adding the matched book to the updated list
                        }

                        updatedBooks.Add(line);
                    }
                }

                if (bookFound)
                {
                    File.WriteAllLines(booksFile, updatedBooks);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Book '{bookTitle}' has been successfully deleted.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: Book '{bookTitle}' not found.");
                    Console.ResetColor();
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error deleting book: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void ListBooks()
        {
            CheckFolder();

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return;
            }

            try
            {
                List<string> books = new List<string>();

                using (StreamReader reader = new StreamReader(booksFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        books.Add(line);  // Add the line (book entry) to the list
                    }
                }

                // Sorting the books manually by title (which is at index 1 of the split line)
                for (int i = 0; i < books.Count - 1; i++)
                {
                    for (int j = i + 1; j < books.Count; j++)
                    {
                        string[] partsI = books[i].Split('|');
                        string[] partsJ = books[j].Split('|');

                        if (string.Compare(partsI[1], partsJ[1], StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            // Swap books[i] and books[j] if books[i] title comes after books[j] in alphabetical order
                            string temp = books[i];
                            books[i] = books[j];
                            books[j] = temp;
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("List of Books:");
                Console.WriteLine("-----------------------------------------------------");
                Console.ResetColor();

                foreach (string book in books)
                {
                    string[] parts = book.Split('|');
                    if (parts.Length >= 7) // Ensure the line has all required fields
                    {
                        Console.WriteLine($"BookID: {parts[0]}");
                        Console.WriteLine($"Title: {parts[1]}");
                        Console.WriteLine($"Author: {parts[2]}");
                        Console.WriteLine($"Publisher: {parts[3]}");
                        Console.WriteLine($"Edition: {parts[4]}");
                        Console.WriteLine($"Year of Publication: {parts[5]}");
                        Console.WriteLine($"Quantity: {parts[6]}");
                        Console.WriteLine("-----------------------------------------------------");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading books file: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void AddBookStock(string bookTitle, int additionalStock)
        {
            CheckFolder(); 

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return;
            }

            List<string> updatedBooks = new List<string>();
            bool bookFound = false;

            try
            {
                using (StreamReader reader = new StreamReader(booksFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 7 && parts[1].Equals(bookTitle, StringComparison.OrdinalIgnoreCase))
                        {
                            int quantity = int.Parse(parts[6]);
                            parts[6] = (quantity + additionalStock).ToString();
                            bookFound = true;
                        }
                        updatedBooks.Add(string.Join("|", parts));
                    }
                }

                if (bookFound)
                {
                    File.WriteAllLines(booksFile, updatedBooks);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfully added {additionalStock} stock(s) to '{bookTitle}'.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: Book '{bookTitle}' not found.");
                    Console.ResetColor();
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error adding stock: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static List<string> SearchBookByTitleWildcard(string partialTitle)
        {
            CheckFolder();
            List<string> searchResults = new List<string>();

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return searchResults;
            }

            try
            {
                using (StreamReader reader = new StreamReader(booksFile))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line into parts to identify the title
                        string[] parts = line.Split('|');

                        if (parts.Length >= 2) // Ensure the line contains at least BookID and Title
                        {
                            string bookTitle = parts[1];
                            if (bookTitle.IndexOf(partialTitle, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                searchResults.Add(line);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading books file: {ex.Message}");
                Console.ResetColor();
            }

            if (searchResults.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"No books found matching the title: {partialTitle}");
                Console.ResetColor();
            }

            return searchResults;
        }

        public static void ViewInventory()
        {
            CheckFolder();

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(booksFile))
                {
                    string? line;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Current Inventory:");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.ResetColor();

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Split the line into parts
                        string[] parts = line.Split('|');
                        if (parts.Length >= 7) // Ensure the line has all required fields
                        {
                            Console.WriteLine($"BookID: {parts[0]}");
                            Console.WriteLine($"Title: {parts[1]}");
                            Console.WriteLine($"Author: {parts[2]}");
                            Console.WriteLine($"Publisher: {parts[3]}");
                            Console.WriteLine($"Edition: {parts[4]}");
                            Console.WriteLine($"Year of Publication: {parts[5]}");
                            Console.WriteLine($"Quantity: {parts[6]}");
                            Console.WriteLine("-----------------------------------------------------");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Warning: Skipping malformed line: {line}");
                            Console.ResetColor();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading books file: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void BorrowBook(string bookTitle, string userId)
        {
            CheckFolder();

            if (!File.Exists(booksFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: The books file does not exist.");
                Console.ResetColor();
                return;
            }

            if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(userId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Book title and User ID cannot be empty.");
                Console.ResetColor();
                return;
            }

            string tempFile = Path.GetTempFileName();
            bool bookFound = false;

            try
            {
                using (StreamReader reader = new StreamReader(booksFile))
                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 7 && parts[1].Equals(bookTitle, StringComparison.OrdinalIgnoreCase))
                        {
                            int quantity = int.Parse(parts[6]);
                            if (quantity > 0)
                            {
                                parts[6] = (quantity - 1).ToString();
                                bookFound = true;

                                // Log borrowing details
                                using (StreamWriter borrowLogWriter = new StreamWriter(@"C:\LibraryData\borrowedBooks.txt", append: true))
                                {
                                    borrowLogWriter.WriteLine($"{userId}|{bookTitle}|{DateTime.Now:yyyy-MM-dd}");
                                }

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Book '{bookTitle}' successfully borrowed by User ID: {userId}.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Error: Book '{bookTitle}' is out of stock.");
                                Console.ResetColor();
                            }
                        }
                        writer.WriteLine(string.Join("|", parts));
                    }
                }

                if (bookFound)
                {
                    File.Delete(booksFile);
                    File.Move(tempFile, booksFile);
                }
                else
                {
                    File.Delete(tempFile);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: Book '{bookTitle}' not found.");
                    Console.ResetColor();
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error borrowing book: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void ViewBorrowedBooksForLibrarians()
        {
            CheckFolder();

            string borrowFile = @"C:\LibraryData\borrowedBooks.txt";
            if (!File.Exists(borrowFile))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No borrowed books records found.");
                Console.ResetColor();
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(borrowFile))
                {
                    string line;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("List of Borrowed Books:");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.ResetColor();

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 3) // Ensure there are at least User ID, Book Title, and Borrowed Date
                        {
                            Console.WriteLine($"User ID: {parts[0]} | Book Title: {parts[1]} | Borrowed On: {parts[2]}");
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading borrowed books: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void ReturnBook(string bookTitle, string userId)
        {
            CheckFolder();

            string borrowFile = @"C:\LibraryData\borrowedBooks.txt";
            if (!File.Exists(borrowFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No borrowed books record found.");
                Console.ResetColor();
                return;
            }

            if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(userId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Book title and User ID cannot be empty.");
                Console.ResetColor();
                return;
            }

            bool bookFound = false;
            string tempBorrowFile = Path.GetTempFileName();

            try
            {
                using (StreamReader reader = new StreamReader(borrowFile))
                using (StreamWriter writer = new StreamWriter(tempBorrowFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 3 && parts[0] == userId && parts[1].Equals(bookTitle, StringComparison.OrdinalIgnoreCase))
                        {
                            bookFound = true;
                            continue; // Skip writing this borrowed book entry
                        }

                        writer.WriteLine(line);
                    }
                }

                if (bookFound)
                {
                    File.Delete(borrowFile);
                    File.Move(tempBorrowFile, borrowFile);

                    // Update book stock
                    string tempBookFile = Path.GetTempFileName();
                    bool stockUpdated = false;

                    using (StreamReader bookReader = new StreamReader(booksFile))
                    using (StreamWriter bookWriter = new StreamWriter(tempBookFile))
                    {
                        string bookLine;
                        while ((bookLine = bookReader.ReadLine()) != null)
                        {
                            string[] bookParts = bookLine.Split('|');
                            if (bookParts.Length >= 7 && bookParts[1].Equals(bookTitle, StringComparison.OrdinalIgnoreCase))
                            {
                                int quantity = int.Parse(bookParts[6]);
                                bookParts[6] = (quantity + 1).ToString();
                                stockUpdated = true;
                            }

                            bookWriter.WriteLine(string.Join("|", bookParts));
                        }
                    }

                    if (stockUpdated)
                    {
                        File.Delete(booksFile);
                        File.Move(tempBookFile, booksFile);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Book '{bookTitle}' successfully returned by User ID: {userId}.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    File.Delete(tempBorrowFile);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: User ID {userId} has not borrowed the book '{bookTitle}'.");
                    Console.ResetColor();
                }
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error returning book: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}