using SimpleBankManagementSystems.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems.Services
{
    class UtilityBankSystem
    {
        public static char leftTopCornerChar = '*';
        public static char rightTopCornerChar = '*';// 187;
        public static char hLineChar = '-';
        public static char vLineChar = '|';
        public static int WidthOfScreen = 100;
        public static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        public static string DataPath = AppDomain.CurrentDomain.BaseDirectory + "data//" ;
        public static string BankAccountDataPath = AppDomain.CurrentDomain.BaseDirectory + "data//BankAccounts//";
        public static string LoginDataFile = @"login.txt";
        public static char separator = '|';
        
        
       public void DisplayWelcomeString()
        {
            string welcomeStr = "WELCOME TO SIMPLE BANKING SYSTEM";
            Console.Write(UtilityBankSystem.leftTopCornerChar);
            Console.Write(new string(UtilityBankSystem.hLineChar, UtilityBankSystem.WidthOfScreen - 2));
            Console.WriteLine(UtilityBankSystem.rightTopCornerChar);

            Console.Write(UtilityBankSystem.vLineChar);
            Console.Write(new string(' ', (UtilityBankSystem.WidthOfScreen - welcomeStr.Length - 2) / 2));
            Console.Write(welcomeStr);
            Console.Write(new string(' ', (UtilityBankSystem.WidthOfScreen - welcomeStr.Length - 2) / 2));
            Console.WriteLine(UtilityBankSystem.vLineChar);
        }
        public void DisplayTitle(string titleStr)
        {            
            Console.Write(UtilityBankSystem.leftTopCornerChar);
            Console.Write(new string(UtilityBankSystem.hLineChar, 98));
            Console.WriteLine(UtilityBankSystem.rightTopCornerChar);

            Console.Write(" ");
            Console.Write(new string(' ', (UtilityBankSystem.WidthOfScreen - titleStr.Length - 2) / 2));
            Console.Write(titleStr);
            Console.Write(new string(' ', (UtilityBankSystem.WidthOfScreen - titleStr.Length - 2) / 2));
            Console.WriteLine(" ");

            Console.Write(" ");
            Console.Write(new string(' ', 98));
            Console.WriteLine(" ");
        } 
        public void DisplayIndent()
        {
            Console.Write(new string(' ', 30));
        }
        /// <summary>
        /// This method is to return a list of UserLogins in file data login.txt
        /// login.txt file: each user login is stored in each line , separate by characrter ';'
        /// All spaces at beginning line, end line, before or after separate char will be remove
        /// Name and Password are case sensitive 
        /// For example : 
        /// name1;password1
        /// name2;password2
        /// </summary>
        /// <returns>List<UserLogin></returns>
        public List<UserLogin> GetUserLoginList()
        {
            List<UserLogin> list = new List<UserLogin>();
            try
            {
                
                if (!Directory.Exists(DataPath))
                {
                    Directory.CreateDirectory(DataPath);
                }
                string filePath =  DataPath + LoginDataFile;
                if (File.Exists(filePath))
                {
                    foreach (string line in File.ReadLines(filePath))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            UserLogin user = new UserLogin();
                            string[] items = line.Split('|');
                            if (items.Length >= 2)
                            {
                                user.UserName = items[0];
                                user.Password = items[1];
                                list.Add(user);
                            }
                        }
                    }                    
                }
                
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }
            return list;
        }
        /// <summary>
        /// This method is to return a list of BankAccount 
        /// </summary>
        /// <returns>List of BankAccount </returns>
        public List<BankAccount> GetBankAccountList()
        {
            List<BankAccount> list = new List<BankAccount>();

            return list;
        }
        /// <summary>
        /// This method is to validate a input string is a valid is email or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// This method is to send email with bank detail to customer
        /// </summary>
        /// <param name="email"></param>
        /// <param name="bankAccount"></param>
        public void SendEmail( BankAccount bankAccount)
        {
            string content = "A bank account has been created for you. Please find the below for your bank account detail. <br/> ";
            content += "Account number: <b>" + bankAccount.AccountNumber + "</b><br/>";
            content += "Balance: <b>" + bankAccount.Balance + "</b><br/>";
            content += "First name: <b>" + bankAccount.FirstName+ "</b><br/>";
            content += "Last name: <b>" + bankAccount.LastName+ "</b><br/>";
            content += "Address: <b>" + bankAccount.Address+ "</b><br/>";
            content += "Phone: <b>" + bankAccount.Phone + "</b><br/>";
            content += "Email: <b>" + bankAccount.Email+ "</b><br/>";
            content += "If you require any further information, please contact Thi Bich Phuong Ngo, 0413 948 650 <p>";
            content += "Best Regards<br/>Simple Bank ";
            string subject = "Simple Bank - A new bank account has been created for you ";
            SendEmail(bankAccount.Email, content, subject);       
        }
        /// <summary>
        /// This method to send email to a given email with a content and subject 
        /// Sender email is my email: 11885744@student.uts.edu.au
        /// Via smtp office365 port 587
        /// 
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="content"></param>
        /// <param name="subject"></param>
        public void SendEmail(string emailTo, string content, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress("11885744@student.uts.edu.au");
                mailMessage.Body = content;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(emailTo);
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("11885744@student.uts.edu.au", "Phu7o75ng@")
                };

                smtpClient.Send(mailMessage);
                DisplayIndent();
                Console.WriteLine("Sent bank detail to {0}. Please check your email.", emailTo);
            }
            catch (Exception ex)
            {
                DisplayIndent();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
