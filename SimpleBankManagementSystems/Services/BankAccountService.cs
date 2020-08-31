using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleBankManagementSystems.Models;
using System.IO;

namespace SimpleBankManagementSystems.Services
{
    class BankAccountService
    {
        UtilityBankSystem utility = new UtilityBankSystem();
        /// <summary>
        /// This method is to search Bank Account by a given account number from data files
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>BankAccount found or null</returns>
        public BankAccount Search(int accountNumber)
        {
            try
            {
                if (!Directory.Exists(UtilityBankSystem.BankAccountDataPath))
                {
                    return null;
                }
                string[] fullPathFileNames = Directory.GetFiles(UtilityBankSystem.BankAccountDataPath, "*.txt");
                foreach (string fullPathFileName in fullPathFileNames)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fullPathFileName);
                    int number = Convert.ToInt32(fileName);
                    if (number == accountNumber)
                    {
                        return this.LoadBankAccount(UtilityBankSystem.BankAccountDataPath + accountNumber + ".txt");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                utility.DisplayIndent();
                Console.Write("Search bank account error: " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// This function is to create a new bank account
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns>BankAccount if success, otherwise null </returns>
        public BankAccount CreateNewAccount(string firstName, string lastName, string address, string phone, string email)
        {
            int newAccountNumber = generateAccountNumber();            
            BankAccount bankAccount = new BankAccount(newAccountNumber, firstName, lastName, address, phone, email, 0);
            // create file data for this bank account            
            if (WriteAccountDetailToFile(bankAccount))
            {
                return bankAccount;
            }
            return null;
        }
        /// <summary>
        /// This method is to deposit or withdraw an amount to an account
        /// </summary>
        /// <param name="account">BankAccount</param>
        /// <param name="amount">decimal</param>
        /// <returns>bool</returns>
        public bool DispositOrWithdraw(BankAccount account, decimal amount)
        {
            account.Balance +=  amount;
            if (amount > 0)
            {
                // deposit                
                account.BankStatementLines.Add(new BankStatementLine(DateTime.Now.ToString("dd.MM.yyyy"), "Deposit", Math.Abs(amount), account.Balance));
            }else
            {
                // withdraw
                account.BankStatementLines.Add(new BankStatementLine(DateTime.Now.ToString("dd.MM.yyyy"), "Withdraw", Math.Abs(amount), account.Balance));
            }
            return WriteFullAccountToFile(account);
        }
        /// <summary>
        /// This method is to process bank statement of a given account
        /// 1. Print 5 last statements to screen
        /// 2. Email 5 last statemts to email account
        /// </summary>
        /// <param name="account"></param>
        public void ProcessBankStatement(BankAccount account)
        {
            string content = "";
            // Display Bank Statement Lines
            utility.DisplayTitle("Bank Statements");
            content = "";
            utility.DisplayIndent();
            Console.WriteLine("Account Number: {0}", account.AccountNumber);
            content += "Bank Statement of account <b>" + account.AccountNumber + "</b><br/>";
            if (account.BankStatementLines.Count == 0)
            {
                utility.DisplayIndent();
                Console.WriteLine("This account does not have any transaction.");
                content += "This account does not have any transaction.";
            }
            else
            {
                utility.DisplayIndent();
                Console.WriteLine("Five last statements of this account ");
                content += "Five last statements of this account<br/>";
                int index = account.BankStatementLines.Count - 5 ;
                if (index < 0)
                {
                    index = 0;
                }
                int numberOfPrintedStatement = 1;
                while ((index < account.BankStatementLines.Count ) )
                {
                    utility.DisplayIndent();
                    Console.WriteLine("{0}. Date: {1}, {2}: {3}, Balance: {4}" , numberOfPrintedStatement, account.BankStatementLines[index].DateStatement, account.BankStatementLines[index].DepositOrWithdraw, account.BankStatementLines[index].Amount, account.BankStatementLines[index].Balance);
                    content +=  (numberOfPrintedStatement +". Date: " + account.BankStatementLines[index].DateStatement +", " + account.BankStatementLines[index].DepositOrWithdraw + ":" +  account.BankStatementLines[index].Amount + ", balance: " + account.BankStatementLines[index].Balance + "<br/>");
                    numberOfPrintedStatement++;
                    index++;
                }

            }
            //Email Bank Statement Lines
            string subject = "Bank account statement";
            utility.SendEmail(account.Email, content, subject);

        }
        /// <summary>
        /// This method is to delete data file by a given account number
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public bool DeleteAccount(int accountNumber)
        {
            try
            {
                File.Delete(UtilityBankSystem.BankAccountDataPath + accountNumber + ".txt");
            }
            catch (Exception ex)
            {
                utility.DisplayIndent();
                Console.WriteLine("Could not delete this account, error: " + ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// This method is to generate a random number between 100000 and 999999 but not used for account number yet
        /// </summary>
        /// <returns>an account number or -1 </returns>        
        private int generateAccountNumber()
        {
            Random random = new Random();
            int randomInt ;
            do
            {
               randomInt = random.Next(100000, 99999999);                               
            } while (File.Exists(UtilityBankSystem.BankAccountDataPath + randomInt + ".txt"));
            return randomInt;
        }
        /// <summary>
        /// This method is to load account detail and statement lines from a given data file into account. 
        /// If file name not found then return null
        /// </summary>
        /// <param name="fileName">String</param>
        /// <returns>BankAccount</returns>
        private BankAccount LoadBankAccount(string fileName)
        {
            try
            {
                BankAccount bankAccount = new BankAccount();
                
                if (File.Exists(fileName))
                {
                    foreach (string line in File.ReadLines(fileName))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] items = line.Split(UtilityBankSystem.separator);                            
                            if (items.Count() >= 2)
                            {
                                if (line.StartsWith("First Name" + UtilityBankSystem.separator))
                                {
                                    bankAccount.FirstName = items[1];
                                }
                                else if (line.StartsWith("Last Name" + UtilityBankSystem.separator))
                                {
                                    bankAccount.LastName = items[1];

                                }
                                else if (line.StartsWith("Address" + UtilityBankSystem.separator))
                                {
                                    bankAccount.Address = items[1];                                    
                                }
                                else if (line.StartsWith("Phone" + UtilityBankSystem.separator))
                                {
                                    bankAccount.Phone = items[1];                                    
                                }
                                else if (line.StartsWith("Email" + UtilityBankSystem.separator))
                                {
                                    bankAccount.Email = items[1];
                                }
                                else if (line.StartsWith("AccountNo" + UtilityBankSystem.separator))
                                {
                                    bankAccount.AccountNumber = Convert.ToInt32(items[1]);
                                }
                                else if (line.StartsWith("Balance" + UtilityBankSystem.separator))
                                {
                                    bankAccount.Balance = Convert.ToDecimal(items[1]);
                                }
                                else
                                {
                                    // starting with statement line
                                    if (items.Count() >= 4)
                                    {
                                        bankAccount.BankStatementLines.Add(new BankStatementLine(items[0], items[1], Convert.ToDecimal(items[2]), Convert.ToDecimal(items[3])));
                                    }
                                }
                            }
                            
                            
                        }
                    }
                }else
                {
                    return null;
                }
                return bankAccount;
            }
            catch (Exception ex)
            {

                Console.Write("LoadBankAccount error: " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// This method is to write account detail only to file data
        /// </summary>
        /// <param name="account"></param>
        /// <returns>bool</returns>
        private bool WriteAccountDetailToFile(BankAccount account)
        {
            try
            {
                if (!Directory.Exists(UtilityBankSystem.BankAccountDataPath))
                {
                    Directory.CreateDirectory(UtilityBankSystem.BankAccountDataPath);
                }
                string fileBankAccount = UtilityBankSystem.BankAccountDataPath + account.AccountNumber + ".txt";
                string str = "First Name" + UtilityBankSystem.separator + account.FirstName + "\n";
                str += "Last Name" + UtilityBankSystem.separator + account.LastName + "\n";
                str += "Address" + UtilityBankSystem.separator + account.Address + "\n";
                str += "Phone" + UtilityBankSystem.separator + account.Phone + "\n";
                str += "Email" + UtilityBankSystem.separator + account.Email + "\n";
                str += "AccountNo" + UtilityBankSystem.separator + account.AccountNumber + "\n";
                str += "Balance" + UtilityBankSystem.separator + account.Balance + "\n";
                File.WriteAllText(fileBankAccount, str);
                return true;
            }
            catch (Exception ex)
            {
                utility.DisplayIndent();
                Console.WriteLine("Coud not write account detail to file, error: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// This method is to write account detail and statement lines to file data
        /// </summary>
        /// <param name="account"></param>
        /// <returns>bool</returns>
        private bool WriteFullAccountToFile(BankAccount account)
        {

            if (WriteAccountDetailToFile(account))
            {
                string fileName = UtilityBankSystem.BankAccountDataPath + account.AccountNumber + ".txt";
                string allLines = "";
                foreach (BankStatementLine item in account.BankStatementLines)
                {
                    DateTime today = DateTime.Now;
                    string str = today.ToString("dd.MM.yyyy") + UtilityBankSystem.separator + item.DepositOrWithdraw + UtilityBankSystem.separator + item.Amount + UtilityBankSystem.separator + item.Balance + "\n";
                    allLines += str;
                }
                try
                {
                    File.AppendAllText(fileName, allLines);
                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured: " + ex.Message);
                }

            }
            return false;
        }

    }
}
