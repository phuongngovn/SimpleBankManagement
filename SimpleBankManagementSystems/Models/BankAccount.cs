using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems.Models
{
    class BankAccount
    {
        public BankAccount()
        {
            this.BankStatementLines = new List<BankStatementLine>();
        }
        public BankAccount(int accountNumber, string firstName, string lastName, string address, string phone, string email, decimal balance)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.Balance = balance;
            this.AccountNumber = accountNumber;
            this.BankStatementLines = new List<BankStatementLine>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int  AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<BankStatementLine> BankStatementLines { get; set; }

    }
    public class BankStatementLine
    {
        public BankStatementLine() { }
        public BankStatementLine(string dateStatement, string depositOrWithdraw, decimal amount, decimal balance)
        {
            this.DateStatement = dateStatement;
            this.DepositOrWithdraw = depositOrWithdraw;
            this.Amount = amount;
            this.Balance = balance;
        }        
        public string DateStatement { get; set; }
        public string DepositOrWithdraw { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance  { get; set; }
    }
    
   
}
