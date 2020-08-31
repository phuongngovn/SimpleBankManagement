using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems.Models
{
    class Transaction
    {
        public int AccountNumber { get; set; }
        public int Deposit { set; get; }
        public double Amount { get; set; }
    }
}
