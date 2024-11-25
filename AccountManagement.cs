using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class UserManagement
    {
        private string? userId, name, role, password;

        //Accessor for UserId, Names and Roles 
        public string? UserID { get => userId; set => userId = value; }
        public string? Name { get => name; set => name = value;}
        public string? Password { get => password; set => password = value; }
        public string? Role { get => role; set => role = value; }

        public override string ToString()
        {
            return $"{UserID}|{Name}|{Password}|{Role}";
        }

        public static void ShowMessageWithDelay(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        
    }

    public static class PasswordManager 
    {
        public static string GetMaskedPassword()
        {
            while (true)
            {
                string password = "";
                ConsoleKeyInfo key;

                do
                {
                    key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password[0..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                } while (true);

                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password cannot be empty! Press any key to try again...");
                    Console.ResetColor();
                    Console.ReadKey(true);
                    Console.Write("Enter Password: ");
                    continue;
                }

                if (password.Length < 8)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Password must be at least 8 characters long! Press any key to try again...");
                    Console.ResetColor();
                    Console.ReadKey(true);
                    Console.Write("Enter Password: ");
                    continue;
                }

                return password;
            }
        }
    }


}
