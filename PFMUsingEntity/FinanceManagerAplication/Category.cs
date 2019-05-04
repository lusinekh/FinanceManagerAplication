using System;
using System.Collections.Generic;

namespace FinanceManagerAplication
{
    public class Category
    {
       public int CategoryId { get; set;}
       public string Title { get; set;}
       public bool IsDebit { get; set; }
       public List<Wallet> Wallets { get; set; }
    }
}
