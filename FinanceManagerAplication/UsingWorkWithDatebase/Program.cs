using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDatebase;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connetionString = @"Data Source=DESKTOP-I87ABNR\SQLEXPRESS;Integrated Security=True;Pooling=False";

            using (DatebaseUse itm = new DatebaseUse())
            {
                var cn = itm.Connetion(connetionString);

                //b.FinanceManagerDateBase(e,"FinanceManagerDateBase");
                string[] CategoryName = { "Category1", "Category2", "Category3" };
                itm.FillTableCategoryRandom(cn, "FinanceManagerDateBase", CategoryName);
            }
        }
    }
}
