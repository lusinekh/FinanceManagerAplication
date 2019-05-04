using FinanceManagerAplication;
using System;
using System.Linq;

namespace ConsoleFinanceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime DAY = DateTime.UtcNow;
           
            Category Category = new Category
            {
                Title = "SHOPING",
                IsDebit = false
            };

            using (var db = new PFMDbContext())
            {
                db.Database.EnsureCreated();              
            }

            using (var db = new PFMDbContext())
            {
               FMFunctions fm = new FMFunctions();
               fm.InitCategoryTable(db, "Credit", true);
               fm.InitCategoryTable(db, "Food", true);
               fm.InitCategoryTable(db, "Food1", true);
               fm.InitWalletTable(db,6789, "", DAY, Category);
               fm.InitWalletTableRandom(db);

               Console.WriteLine(fm.CalculateAmount(db, DateTime.Now, DateTime.Now));

            }
        }
    }
}
