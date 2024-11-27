using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    class StudentMenu
    {
        public void Student()
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
                Console.WriteLine("┌───────────────────────────┐");
                Console.WriteLine("│ 1. List Books             │");
                Console.WriteLine("│ 2. Borrow Book            │");
                Console.WriteLine("│ 3. Return Book            │");
                Console.WriteLine("│ 4. Search Books           │");
                Console.WriteLine("│ 5. Logout                 │");
                Console.WriteLine("│ 0. Save and Exit          │");
                Console.WriteLine("└───────────────────────────┘\n");
                Console.ResetColor();

                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Listing All Books...");
                        Console.ResetColor();

                        FileHandler.ListBooks();

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Borrow a Book");
                        Console.ResetColor();

                        Console.Write("Enter the title of the book you want to borrow: ");
                        string bookTitleToBorrow = Console.ReadLine();

                        Console.Write("Enter your User ID: ");
                        string userId = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(bookTitleToBorrow) && !string.IsNullOrWhiteSpace(userId))
                        {
                            FileHandler.BorrowBook(bookTitleToBorrow, userId);
                        }
                        else
                        {
                            FileHandler.ShowMessageWithDelay("Error: Book title or User ID cannot be empty.", ConsoleColor.Red);
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Return a Book");
                        Console.ResetColor();

                        Console.Write("Enter the title of the book you want to return: ");
                        string bookTitleToReturn = Console.ReadLine();

                        Console.Write("Enter your User ID: "); // Collect the User ID
                        string returnUserId = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(bookTitleToReturn) && !string.IsNullOrWhiteSpace(returnUserId))
                        {
                            FileHandler.ReturnBook(bookTitleToReturn, returnUserId); // Pass both arguments
                        }
                        else
                        {
                            FileHandler.ShowMessageWithDelay("Error: Book title or User ID cannot be empty.", ConsoleColor.Red);
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Search for a Book by Title and/or Category");
                        Console.ResetColor();

                        Console.Write("Enter a keyword to search for books by title: ");
                        string searchKeyword = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(searchKeyword))
                        {
                            FileHandler.ShowMessageWithDelay("Error: Search keyword cannot be empty.", ConsoleColor.Red);
                            break;
                        }

                        // Ask for category (optional)
                        Console.Write("Enter a category to search (leave blank to skip): ");
                        string searchCategory = Console.ReadLine();

                        // Call the updated SearchBook method from FileHandler
                        List<string> searchResults = FileHandler.SearchBook(searchKeyword, searchCategory);

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
                            string message = string.IsNullOrEmpty(searchCategory) ?
                                             $"No books found matching the title: {searchKeyword}" :
                                             $"No books found matching the title: {searchKeyword} and category: {searchCategory}";
                            Console.WriteLine(message);
                            Console.ResetColor();
                        }

                        Console.WriteLine("\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Logging out...");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to return to the Login and Register Menu...");
                        Console.ReadKey();
                        Library library = new Library();
                        library.RunLibraryMenu();
                        break;

                    case "0":
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.WriteLine("Exiting the program... Goodbye!");
                        Console.ResetColor();
                        Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice! Please try again.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;

                }
            }
        }
    }
}
