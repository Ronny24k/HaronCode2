using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Library : BookItem
    {
        UserManagement user = new UserManagement();
    //    private List<LibraryItem> items = new List<LibraryItem>();
        private string? loggedInUser = null;
        private string BooksFile = @"C:\LibraryData\books.txt";
        private string UsersFile = @"C:\LibraryData\users.txt";

        public Library()
        {
           // CreateFolderIfNotExists(BooksFile);
           // CreateFolderIfNotExists(UsersFile);

           // items = LoadBooks();
        }

       public static void ShowMessageWithDelay(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        /* 

        

        public void DeleteBook(string isbn)
        {
            Book book = items.OfType<Book>().FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                items.Remove(book);
                ShowMessageWithDelay($"Book with ISBN {isbn} deleted successfully.", ConsoleColor.Green);
            }
            else if (string.IsNullOrEmpty(isbn))
            {
                ShowMessageWithDelay("Error: ISBN cannot be empty. Please provide all required information to delete a book.", ConsoleColor.Red);
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                return;
            }
            else
            {
                ShowMessageWithDelay($"Error: Book with ISBN {isbn} not found. Please check the ISBN and try again.", ConsoleColor.Red);
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                return;
            }
        }

        public Book SearchBook(string isbn)
        {
            return items.OfType<Book>().FirstOrDefault(b => b.ISBN == isbn);
        }

        public void BorrowBook(string isbn, string userid)
        {
            Book book = SearchBook(isbn);
            if (book != null)
            {
                if (!book.IsBorrowed)
                {
                    book.BorrowItem();
                    ShowMessageWithDelay($"Book {book.Title} borrowed by User {userid}.", ConsoleColor.Green);
                }
                else
                {
                    ShowMessageWithDelay($"Error: Book {book.Title} with ISBN {isbn} is currently borrowed. Please try another book or wait until it's returned.", ConsoleColor.Red);
                    Console.WriteLine("Press any key to return to menu...");
                    Console.ReadKey();
                }
            }
            else
            {
                ShowMessageWithDelay($"Error: Book with ISBN {isbn} not found. Please check the ISBN and try again.", ConsoleColor.Red);
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
            }
        }

        public void ReturnBook(string isbn)
        {
            Book book = SearchBook(isbn);
            if (book != null)
            {
                if (book.IsBorrowed)
                {
                    book.ReturnItem();
                    ShowMessageWithDelay($"Book {book.Title} returned.", ConsoleColor.Green);
                }
                else
                {
                    ShowMessageWithDelay($"Error: Book {book.Title} with ISBN {isbn} was not borrowed. Please check the ISBN and try again.", ConsoleColor.Red);
                    Console.WriteLine("Press any key to return to menu...");
                    Console.ReadKey();
                }
            }
            else
            {
                ShowMessageWithDelay($"Error: Book with ISBN {isbn} not found. Please check the ISBN and try again.", ConsoleColor.Red);
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
            }
        }

       /* public void SaveData()
        {
            try
            {
                File.WriteAllLines(BooksFile, items.OfType<Book>().Select(b => $"{b.ISBN}|{b.Title}|{b.Author}|{b.IsBorrowed}"));
                File.WriteAllLines(UsersFile, users.Select(u => $"{u.UserID}|{u.Name}|{u.Role}"));
                ShowMessageWithDelay("Data saved successfully.", ConsoleColor.Green);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
            }
        } */

        /*  public void ListBooks()
          {
              if (items.Count == 0)
              {
                  Console.WriteLine("No books available in the library.");
              }
              else
              {
                  Console.WriteLine("List of Books:");
                  items.OfType<Book>().ToList().ForEach(b => Console.WriteLine(b));
              }
              ShowMessageWithDelay("Press any key to return to the menu...");
          }



          private List<LibraryItem> LoadBooks()
          {
              List<LibraryItem> loadedBooks = new List<LibraryItem>();

              try
              {
                  if (File.Exists(BooksFile))
                  {
                      string[] lines = File.ReadAllLines(BooksFile);
                      foreach (string line in lines)
                      {
                          string[] parts = line.Split('|');
                          if (parts.Length == 4)
                          {
                              string isbn = parts[0];
                              string title = parts[1];
                              string author = parts[2];
                              bool isBorrowed = bool.Parse(parts[3]);
                              Book book = new Book(isbn, title, author)
                              {
                                  IsBorrowed = isBorrowed
                              };
                              loadedBooks.Add(book);
                          }
                      }
                  }
                  else
                  {
                      File.Create(BooksFile).Dispose();
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine($"Error loading books: {ex.Message}");
                  Console.WriteLine("Press any key to return to menu...");
                  Console.ReadKey();
              }

              return loadedBooks;
          } */



        public void RunLibraryMenu() 
        {
            while (loggedInUser == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                                                   ║");
                Console.WriteLine("║                 ███████╗████████╗ ██████╗██████╗                  ║");
                Console.WriteLine("║                 ██╔════╝╚══██╔══╝██╔════╝██╔══██╗                 ║");
                Console.WriteLine("║                 █████╗     ██║   ██║     ██████╔╝                 ║");
                Console.WriteLine("║                 ██╔══╝     ██║   ██║     ██╔═══╝                  ║");
                Console.WriteLine("║                 ███████╗   ██║   ╚██████╗██║                      ║");
                Console.WriteLine("║                 ╚══════╝   ╚═╝    ╚═════╝╚═╝                      ║");
                Console.WriteLine("║                                                                   ║");
                Console.WriteLine("║         Evangelical Theological College of the Philippines        ║");
                Console.WriteLine("║                     Library Management System                     ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════════╝\n");
                Console.ResetColor();

                Console.ForegroundColor= ConsoleColor.Magenta;
                Console.WriteLine("┌────────────────────┐");
                Console.WriteLine("│ 1. Login           │");
                Console.WriteLine("│ 2. Register        │");
                Console.WriteLine("│ 3. Exit            │");
                Console.WriteLine("└────────────────────┘\n");
                Console.ResetColor();
                Console.Write("Choose an option: ");
                string? initialChoice = Console.ReadLine();

                if (initialChoice == "1")
                {
                    Console.Write("Enter User ID to login: ");
                    user.UserID = Console.ReadLine();
                    if (user.UserID == null)
                    {
                        Console.WriteLine("User ID is left empty.");
                        continue;
                    }

                    Console.Write("Enter Password: ");
                    user.Password = PasswordManager.GetMaskedPassword();

                    loggedInUser = FileHandler.Login(user.UserID, user.Password);

                    if (loggedInUser == null)
                    {
                        ShowMessageWithDelay("Error: Invalid User ID. Please try again.", ConsoleColor.Red);
                    }


                    if (loggedInUser == "student")
                    {
                        Console.WriteLine($"Welcome, {user.UserID}!");
                        StudentMenu studentMenu = new StudentMenu();
                        studentMenu.Student();
                    }

                    if (loggedInUser == "librarian")
                    {
                        Console.WriteLine($"Welcome, {user.UserID}!");
                        LibrarianMenu librarianMenu = new LibrarianMenu();
                        librarianMenu.Librarian();
                    }
                }
                else if (initialChoice == "2")
                {
                    Console.Write("Enter new User ID: ");
                    user.UserID = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(user.UserID))
                    {
                        ShowMessageWithDelay("Error: User ID cannot be blank.", ConsoleColor.Red);
                        continue;
                    }

                    Console.Write("Enter Name: ");
                    user.Name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(user.Name))
                    {
                        ShowMessageWithDelay("Error: Name cannot be blank.", ConsoleColor.Red);
                        continue;
                    }

                    Console.Write("Enter Password: ");
                    user.Password = PasswordManager.GetMaskedPassword();

                    Console.Write("Enter Role (Librarian/Student): ");
                    user.Role = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(user.Role))
                    {
                        ShowMessageWithDelay("Error: Role cannot be blank.", ConsoleColor.Red);
                        continue;
                    }

                    if (user.Role.ToLower() != "librarian" && user.Role.ToLower() != "student")
                    {
                        ShowMessageWithDelay("Error: Role must be 'Librarian' or 'Student'.", ConsoleColor.Red);
                        continue;
                    }

                    //Register the user to the file
                    FileHandler.AddUser(user.UserID, user.Name, user.Password, user.Role);

                    ShowMessageWithDelay("User registered successfully! You can now login with your new account.", ConsoleColor.Green);
                }
                else if (initialChoice == "3")
                {
                    ShowMessageWithDelay("Exiting Program", ConsoleColor.Green);
                    return;
                }
                else
                {
                    ShowMessageWithDelay("Invalid choice. Please enter 1 or 2.", ConsoleColor.Red);
                }

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }

        }
    }
}
