using SimpleBankManagementSystems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems.Services
{
    class MainMenu
    {
        UtilityBankSystem utility = new UtilityBankSystem();
        BankAccountService bankService = new BankAccountService();
        /// <summary>
        /// This method is to display main menu  with 7 options and allow user to select an option from 1 to 7 
        /// </summary>
        /// <returns>an int between 1 and 7</returns>
        public void DisplayMenu()
        {
            int result = 0;
            
            utility.DisplayTitle("Main Menu");
            utility.DisplayIndent();
            Console.WriteLine("1. Create a new account");
            utility.DisplayIndent();
            Console.WriteLine("2. Search for an account");
            utility.DisplayIndent();
            Console.WriteLine("3. Deposit");
            utility.DisplayIndent();
            Console.WriteLine("4. Withdraw");
            utility.DisplayIndent();
            Console.WriteLine("5. Account statement");
            utility.DisplayIndent();
            Console.WriteLine("6. Delete account");
            utility.DisplayIndent();
            Console.WriteLine("7. Exit");
            Console.WriteLine("");
            do
            {
                utility.DisplayIndent();
                Console.Write("Enter your choice (1-7): ");
                result = Convert.ToInt32(Console.ReadLine());
            } while ((result < 1) || (result > 7));

            switch (result)
            {
                case 1:
                    DisplayCreateNewAccount();
                    break;
                case 2:
                    DisplaySearchAccount();
                    break;
                case 3:
                    DisplayDeposit();
                    break;
                case 4:
                    DisplayWithdraw();
                    break;
                case 5:
                    DisplayBankStatement();
                    break;
                case 6:
                    DisplayDeleteAccount();
                    break;
                case 7:                    
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        ///  This method is called when user enter number 5 from main menu to perform Display bank statement functionality
        /// </summary>
        public void DisplayBankStatement()
        {
            utility.DisplayTitle("Bank Statement");
            BankAccount accountFound = SearchAccount();
            if (accountFound == null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not find account with this account number");
            }else
            {
                bankService.ProcessBankStatement(accountFound);
            }            
            BackToMainMenu();
        }
        /// <summary>
        /// This method is called when user select option 1 from main menu to perform Create new Account functionality
        /// This method is to display screen for user enter information to create a new account
        /// </summary>
        public void DisplayCreateNewAccount()
        {
            string firstName , lastName, email, address, phone;
            utility.DisplayTitle("Create New Account");
            utility.DisplayTitle("Enter The Details");

            bool cont = true;
            do
            {                
                utility.DisplayIndent();
                Console.Write("First Name: ");
                firstName = Console.ReadLine();
                if (string.IsNullOrEmpty(firstName))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("First name is required field.");
                }
                else
                {
                    cont = false;
                }
            } while (cont);

            // Enter Last Name
            cont = true;
            do
            {
                utility.DisplayIndent();
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
                if (string.IsNullOrEmpty(lastName))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Last name is required field.");
                    
                }
                else
                {
                    cont = false;
                }
            } while (cont);

            // Enter Address
            cont = true;
            do
            {
                
                utility.DisplayIndent();
                Console.Write("Address: ");
                address = Console.ReadLine();
                if (string.IsNullOrEmpty(address))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Address is required field.");
                }
                else
                {
                    cont = false;
                }
            } while (cont);

            // Enter phone number
            cont = true;
            do
            {               
                utility.DisplayIndent();
                Console.Write("Phone: ");
                phone = Console.ReadLine();
                if (phone.Length > 10)
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Phone number should be less than 10 ");
                }
                else if (!phone.All(char.IsDigit))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Phone number contains only digits");
                }
                else
                {
                    cont = false;
                }

            } while (cont);

            // Enter Email
            cont = true;
            do
            {               
                utility.DisplayIndent();
                Console.Write("Email: ");
                email = Console.ReadLine();
                if (string.IsNullOrEmpty(email))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Email is a required field.");
                }else if (!utility.IsValidEmail(email))
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Email is not valid format.");
                }else 
                {
                    cont = false;
                }

            } while (cont);

            BankAccount bankAccount = bankService.CreateNewAccount(firstName, lastName, address, phone, email);
           
            if (bankAccount != null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Your account number is: {0}", bankAccount.AccountNumber);
                utility.DisplayIndent();
                Console.WriteLine("Please wait for sending email...");
                utility.SendEmail(bankAccount);
            }else
            {
                utility.DisplayIndent();
                Console.WriteLine("Cannot create new account.");
            }
            BackToMainMenu();
        }
        /// <summary>
        /// This method is called when user select option 2 from main menu.
        /// The method is to display screen for user to search an account by account number and then display the found account details
        /// </summary>
        public void DisplaySearchAccount()
        {
            utility.DisplayTitle("Search Account");            
            BankAccount accountFound = SearchAccount();
            if (accountFound == null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not find account with this account number");
            }else
            {
                DisplayBankAccountDetail(accountFound);
            }
               
            BackToMainMenu();
        }
        /// <summary>
        /// This method is called when user select option 3 from main menu.
        /// The method is to display screen for user to deposit money into an account
        /// </summary>
        public void DisplayDeposit()
        {
            utility.DisplayTitle("Deposit");            
            BankAccount accountFound = SearchAccount();
            decimal amount = InputAmountMonye();
            if (accountFound == null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not find account with this account number");
            }
            else
            {
                bankService.DispositOrWithdraw(accountFound, amount);
            }            
            BackToMainMenu();
        }
        /// <summary>
        /// This method is called when user select option 4 from main menu.
        /// The method is to display screen for user to withdraw money into an account
        /// </summary>
        public void DisplayWithdraw()
        {
            utility.DisplayTitle("Withdraw");
            BankAccount accountFound = SearchAccount();
            if (accountFound == null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not find account with this account number");
            }else
            {
                if (accountFound.Balance == 0)
                {
                    utility.DisplayIndent();
                    Console.WriteLine("The balance of this account is zero soo cannot to continue a withdraw action.");
                    BackToMainMenu();
                }
                decimal amount = 0;
                bool cont = true;
                do
                {
                    amount = InputAmountMonye();
                    if (amount > accountFound.Balance)
                    {
                        utility.DisplayIndent();
                        Console.WriteLine("Your balance is not enough to withdraw this amount, please try another amount.");
                    }
                    else
                    {
                        cont = false;
                    }
                } while (cont);

                bankService.DispositOrWithdraw(accountFound, (-1 * amount));
            }
            
            BackToMainMenu();
        }
        public void DisplayDeleteAccount()
        {
            utility.DisplayTitle("Delete Account");
            BankAccount accountFound = SearchAccount();
            if (accountFound == null)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not find account with this account number");

            }else
            {
                // found account
                this.DisplayBankAccountDetail(accountFound);
                utility.DisplayIndent();
                Console.WriteLine("Are you sure to delete this account (Y/N)?");
                string ch = Console.ReadLine();
                if ((ch == "Y") || (ch == "y"))
                {
                    if (bankService.DeleteAccount(accountFound.AccountNumber))
                    {
                        utility.DisplayIndent();
                        Console.WriteLine("This account has been deteled successully.");
                     }
                 }
            }
            BackToMainMenu();           

        }
        private decimal InputAmountMonye()
        {
            decimal amount = 0;
            bool cont = true;
            do
            {
                try
                {
                    utility.DisplayIndent();
                    Console.Write("Amount: ");
                    amount = Convert.ToDecimal(Console.ReadLine());
                    if (amount <= 0)
                    {
                        utility.DisplayIndent();
                        Console.WriteLine("Amount must be a valid number greater than 0 ");
                    }
                    else
                    {
                        cont = false;
                    }
                }
                catch (Exception ex)
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Amount must be a valid number and greater than 0, error:  {0}", ex.Message);
                }
            } while (cont);
            return amount;
        }
        /// <summary>
        /// This method is to clear screen and then re-display the main menu
        /// </summary>
        private void BackToMainMenu()
        {
            utility.DisplayIndent();
            Console.WriteLine("Please press any key to continue");
            Console.ReadKey();
            Console.Clear();
            DisplayMenu();
        }
        /// <summary>
        /// This method is to display screen for user enter account number and return an account was found or null
        /// </summary>
        /// <returns>a BankAccount found or null</returns>
        private BankAccount SearchAccount()
        {
            int accountNumber = 0;
            bool cont = true;
            BankAccount accountFound = null;
            do
            {
                try
                {
                    utility.DisplayIndent();
                    Console.Write("Account number: ");
                    accountNumber = Convert.ToInt32(Console.ReadLine());
                    if ((accountNumber < 100000) || (accountNumber >= 99999999))
                    {
                        utility.DisplayIndent();
                        Console.WriteLine("Account number must be a number and has from 6 to 8 digits. Please enter a valid account number.");
                    }
                    else
                    {
                        accountFound = bankService.Search(accountNumber);
                        cont = false;                        
                    }

                }
                catch (Exception ex)
                {
                    utility.DisplayIndent();
                    Console.WriteLine("Invalid number format for account number, error: " + ex.Message);
                }

            } while (cont);
            return accountFound;
        }
        
        /// <summary>
        /// This method is to diplay bank account detail
        /// </summary>
        /// <param name="account">BankAccount</param>
        private void DisplayBankAccountDetail(BankAccount account)
        {
            utility.DisplayTitle("Account Details");
            utility.DisplayIndent();
            Console.WriteLine("Account number: {0}", account.AccountNumber);
            utility.DisplayIndent();
            Console.WriteLine("Balance: {0}", account.Balance);
            utility.DisplayIndent();
            Console.WriteLine("First name: {0}", account.FirstName);
            utility.DisplayIndent();
            Console.WriteLine("Last name: {0}", account.LastName);
            utility.DisplayIndent();
            Console.WriteLine("Address: {0}", account.Address);
            utility.DisplayIndent();
            Console.WriteLine("Phone: {0}", account.Phone);
            utility.DisplayIndent();
            Console.WriteLine("Email: {0}", account.Email);
           
        }
        public bool PressEscapeToBackMainMenu()
        {
            ConsoleKeyInfo ch ;
            utility.DisplayIndent();
            Console.WriteLine("Please press ESC to back main menu or another key to continue");
            ch = Console.ReadKey();            
            if ((byte)ch.KeyChar == 27)
            {
                return true;
            }
            return false;
        }
    }
}
