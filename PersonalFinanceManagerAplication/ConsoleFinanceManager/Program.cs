using FinanceManagerDatebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFinanceManager
{
    class Program
    {
        static void Main(string[] args)
        {


            string connectionString = @"Data Source=DESKTOP-I87ABNR\SQLEXPRESS;Integrated Security=True;Pooling=False";

            using (PersonalFinanceManagment pfm = new PersonalFinanceManagment(connectionString))
            {
                pfm.CreateDbAsync("FinanceManagerDateBase").Wait(); 

                pfm.CreateCategoryTableAsync("FinanceManagerDateBase").Wait();

                string[] e = { "Food", "Salary", "kredit" };

                pfm.FinanceManagerDatebaseAsync("FinanceManagerDateBase").Wait();

                pfm.InitCategoryTableRandom("FinanceManagerDateBase", e).Wait();  
                           
                var r = pfm.GateCategoryIdArray("FinanceManagerDateBase");   
                          
                DateTime dt = DateTime.Now;
                DateTime dr = DateTime.Parse("12.06.2017");         

                Console.WriteLine(pfm.AmountCalculate("FinanceManagerDateBase", dr, dt));

                pfm.DeleteItmById("FinanceManagerDateBase", "Category", "16851173-87dd-4f79-9e88-026bbf41d287").Wait();

            }
       }
    }
}
