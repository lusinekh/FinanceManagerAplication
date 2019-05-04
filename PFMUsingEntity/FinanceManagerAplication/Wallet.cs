using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagerAplication
{
    public class Wallet
    {
        public int WalletId { set; get; }
        public int Amount { set; get; }
        public string Comment { set; get; }
        public DateTime Day { set; get; }        
        public  DateTime DateCreated { set; get; }
        public int CategoryId { set; get;}
        public Category Category { get; set; }
    }
}
