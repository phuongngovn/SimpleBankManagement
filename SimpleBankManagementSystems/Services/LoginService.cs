using SimpleBankManagementSystems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems.Services
{
    class LoginService
    {
        
        UtilityBankSystem utility = new UtilityBankSystem();
        /// <summary>
        /// This method is to display login sreen and allow user enter username and password
        /// and then verify the username and password.
        /// </summary>
        public void DisplayLoginMenu()
        {
            utility.DisplayWelcomeString();
            utility.DisplayTitle("LOGIN TO START");
            string userName;
            string password;
            Tuple<bool, string> resultLogin = new Tuple<bool, string>(true, "");
            do
            {
                if (!resultLogin.Item1)
                {
                    utility.DisplayIndent();
                    Console.WriteLine("{0}", resultLogin.Item2);
                }
                do
                {
                    utility.DisplayIndent();
                    Console.Write("User name:");
                    userName = Console.ReadLine();
                } while (string.IsNullOrEmpty(userName));

                // Enter password
                do
                {
                    utility.DisplayIndent();
                    Console.Write("Password:");
                    password = ReadPassword();
                } while (string.IsNullOrEmpty(password));
                resultLogin = Login(userName, password);
            } while (!resultLogin.Item1);
            utility.DisplayIndent();
            Console.WriteLine( "Welcome {0} to Simple Bank Management System", userName);

        }
        /// <summary>
        /// This function is to login to the system
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>a tuple of bool and message if login failed</returns>
        public Tuple<bool,string> Login(string username, string password)
        {
            List<UserLogin> list = this.utility.GetUserLoginList();
            if (list.Count == 0)
            {
                return new Tuple<bool, string>(false, "The list of user login is empty.");
            }
            UserLogin found = list.Where(r => r.UserName.Equals(username) && r.Password.Equals(password)).FirstOrDefault();
            if (found == null)
            {
                return new Tuple<bool, string>(false, "Invalid user name or password, please try again.");
            }
            return new Tuple<bool, string>(true, "");
        }
        private string ReadPassword(char mask = '*')
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write(mask);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);

                    if (Console.CursorLeft == 0)
                    {
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        Console.Write(' ');
                        Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                    }
                    else Console.Write("\b \b");
                }
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}
