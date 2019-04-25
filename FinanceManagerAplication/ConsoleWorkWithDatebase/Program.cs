using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDatebase;

namespace ConsoleWorkWithDatebase
{
    class Program
    {
        static void Main(string[] args)
        {
            string connetionString = @"Data Source=DESKTOP-I87ABNR\SQLEXPRESS;Integrated Security=True;Pooling=False";
            DatebaseUse itm = new DatebaseUse();
            string[] CategoryName = { "Category1", "Category1", "Category3" }; 
            using (SqlConnection cnn = itm.Connetion(connetionString))
            {
                itm.FinanceManagerDateBase(cnn, "FinanceManagerDateBase14");
                itm.FillTableCategoryRandom(cnn, "FinanceManagerDateBase14", CategoryName);

            }
        }
    }
}
