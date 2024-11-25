using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class LibrarianMenu 
    {
        public void Librarian()
        {
            while (true)
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

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("┌───────────────────────────────┐");
                Console.WriteLine("│ 1. Add Book                   │");
                Console.WriteLine("│ 2. Delete Book                │");
                Console.WriteLine("│ 3. List Books                 │");
                Console.WriteLine("│ 4. Add User                   │");
                Console.WriteLine("│ 5. Delete User                │");
                Console.WriteLine("│ 6. List Users                 │");
                Console.WriteLine("│ 7. Search Books               │");
                Console.WriteLine("│ 8. View Inventory             │");
                Console.WriteLine("│ 9. Add Book Stocks            │");
                Console.WriteLine("│ 10. View Users Borrowed Books │");
                Console.WriteLine("│ 11. Logout                    │");
                Console.WriteLine("│ 0. Save and Exit              │");
                Console.WriteLine("└───────────────────────────────┘\n");
                Console.ResetColor();

                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Add a New Book");
                        Console.ResetColor();

                        Console.Write("Enter Book Title: ");
                        string title = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(title))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Book title cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter Author: ");
                        string author = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(author))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Author cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter Year of Publication: ");
                        if (!int.TryParse(Console.ReadLine(), out int yearOfPublication) || yearOfPublication <= 0)
                        {
                            FileHandler.ShowMessageWithDelay("Error: Invalid year of publication.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter Publisher: ");
                        string publisher = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(publisher))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Publisher cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter Edition: ");
                        string edition = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(edition))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Edition cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter Quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                        {
                            FileHandler.ShowMessageWithDelay("Error: Invalid quantity.", ConsoleColor.Red);
                            break;
                        }

                        FileHandler.AddBook(title, author, yearOfPublication, publisher, edition, quantity);
                        break;
                    case "2":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Delete a Book");
                        Console.ResetColor();

                        Console.Write("Enter the title of the book to delete: ");
                        string bookTitleToDelete = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(bookTitleToDelete))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Book title cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        // Call the DeleteBook method from FileHandler
                        FileHandler.DeleteBook(bookTitleToDelete);
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Listing All Books...");
                        Console.ResetColor();

                        FileHandler.ListBooks();

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "4":
                        UserManagement user = new UserManagement();
                        Console.Write("Enter new User ID: ");
                        user.UserID = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(user.UserID))
                        {
                            FileHandler.ShowMessageWithDelay("Error: User ID cannot be blank.", ConsoleColor.Red);
                            continue;
                        }

                        Console.Write("Enter Name: ");
                        user.Name = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(user.Name))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Name cannot be blank.", ConsoleColor.Red);
                            continue;
                        }

                        Console.Write("Enter Password: ");
                        user.Password = PasswordManager.GetMaskedPassword();

                        Console.Write("Enter Role (Librarian/Student): ");
                        user.Role = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(user.Role))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Role cannot be blank.", ConsoleColor.Red);
                            continue;
                        }

                        if (user.Role.ToLower() != "librarian" && user.Role.ToLower() != "student")
                        {
                            FileHandler.ShowMessageWithDelay("Error: Role must be 'Librarian' or 'Student'.", ConsoleColor.Red);
                            continue;
                        }

                        //Register the user to the file
                        FileHandler.AddUser(user.UserID, user.Name, user.Password, user.Role);

                        FileHandler.ShowMessageWithDelay("User registered successfully! You can now login with your new account.", ConsoleColor.Green);
                        break;
                    case "5":
                        List<string> list = new List<string>();
                        list = FileHandler.LoadUsers();
                        foreach (string users in list)
                        {
                            Console.WriteLine($"{users}");
                        }
                        Console.Write("Enter User ID to delete: ");
                        string userID = Console.ReadLine();
                        FileHandler.DeleteUser(userID);
                        Console.ReadKey();
                        break;
                    case "6":
                        list = new List<string>();
                        list = FileHandler.LoadUsers();
                        foreach (string users in list)
                        {
                            Console.WriteLine($"{users}");
                        }
                        Console.ReadKey();
                        break;
                    case "7":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Search for a Book by Title");
                        Console.ResetColor();

                        Console.Write("Enter a keyword to search for books: ");
                        string searchKeyword = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(searchKeyword))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Search keyword cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        // Call the SearchBookByTitleWildcard method from FileHandler
                        List<string> searchResults = FileHandler.SearchBookByTitleWildcard(searchKeyword);

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Search Results:");
                        Console.ResetColor();

                        if (searchResults.Count > 0)
                        {
                            foreach (string book in searchResults)
                            {
                                Console.WriteLine(book);
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"No books found matching the keyword: {searchKeyword}");
                            Console.ResetColor();
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "8":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Viewing Inventory...");
                        Console.ResetColor();

                        // Call the FileHandler's ViewInventory method
                        FileHandler.ViewInventory();
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "9":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Add Stock to a Book");
                        Console.ResetColor();

                        Console.Write("Enter the title of the book to add stock: ");
                        string bookTitle = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(bookTitle))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Book title cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        Console.Write("Enter the quantity to add: ");
                        if (!int.TryParse(Console.ReadLine(), out int additionalStock) || additionalStock < 0)
                        {
                            FileHandler.ShowMessageWithDelay("Error: Invalid quantity.", ConsoleColor.Red);
                            break;
                        }

                        // Call the AddBookStock method from FileHandler
                        FileHandler.AddBookStock(bookTitle, additionalStock);

                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "10":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Viewing Borrowed Books...");
                        Console.ResetColor();
                        FileHandler.ViewBorrowedBooksForLibrarians();
                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "11":
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Logging out...");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to return to the Login and Register Menu...");
                        Console.ReadKey();
                        Library library = new Library();
                        library.RunLibraryMenu();
                        return;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting Program... Goodbye!");
                        Console.ResetColor();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine("Invalid choice! Please try again.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }


        }
    }
}
