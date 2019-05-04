using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace FinanceManagerAplication
{
    public class FMFunctions
    {
       static Random rd = new Random();
       public void InitCategoryTable(PFMDbContext dt, string title,bool IsDebit)
        {
            Category categories = new Category
            {
                Title = title,
                IsDebit = IsDebit
            };

            if(dt!=null)
            {
                dt.Add(categories);
                dt.SaveChanges();
            }
        }

        public void InitWalletTable(PFMDbContext dt,int amount, string Comment, DateTime day,Category category)
        {
            Wallet Wallet = new Wallet
            {
                Amount = amount,
                Comment = Comment,
                Category = category,
                Day = day,
                DateCreated = DateTime.Now
            };

            if (dt != null)
            {
                dt.Add(Wallet);
                dt.SaveChanges();
            }
        }
        public void InitWalletTableRandom(PFMDbContext dt)
        {
            var year = rd.Next(2000, 2019);
            var mounth = rd.Next(1, 13);
            var day = rd.Next(1, 28);
            var category = dt.Categories.Select(x => x).ToList();
            DateTime start = new DateTime(year, mounth, day);
            int index = rd.Next(0, category.Count - 1);

            for (int i=0;i<500;i++)
            {
                Wallet Wallet = new Wallet
                {
                    Amount = rd.Next(100, 5000),
                    Comment = " ",
                    Category = category[index],
                    Day = start,
                    DateCreated = DateTime.Now
                };
                dt.Add(Wallet);
            }

            if (dt != null)
            {
               dt.SaveChanges();
            }

            for (int i = 0; i < 500; i++)
            {

                Wallet Wallet = new Wallet
                {
                    Amount = rd.Next(100, 5000),
                    Comment = " ",
                    Category = category[index],
                    Day = start,
                    DateCreated = DateTime.Now
                };
                dt.Add(Wallet);
            }

            if (dt != null)
            {
                dt.SaveChanges();
            }
        }
        public void DeleteWalletTable(PFMDbContext dt, Wallet wallet)
        {
            dt.Wallets.Remove(wallet);
            dt.SaveChanges();
        }
        public void DeleteCategorieTable(PFMDbContext dt, Category Category)
        {
            dt.Categories.Remove(Category);
            dt.SaveChanges();
        }
        public void DeleteWalletTableRang(PFMDbContext dt, Wallet[] wallet)
        {           
            dt.Wallets.RemoveRange(wallet);
            dt.SaveChanges();
        }
        public void DeleteCategorieTableRang(PFMDbContext dt, Category[] Category)
        {
            dt.Categories.RemoveRange(Category);
            dt.SaveChanges();
        }
        public int CalculateAmount(PFMDbContext db, DateTime start, DateTime end)
        {

            try
            {
                var WalletsCategory = db.Wallets.Where(x => x.Day > start && x.Day > end).Join(db.Categories.DefaultIfEmpty(),
                    u => u.CategoryId,
                    d => d.CategoryId,
                    (Wallet, Category) => (Wallet.Amount)).Select(x => x).Sum();


               
                return WalletsCategory;
            }

            catch(Exception ex)
            {

                Console.WriteLine(ex);
                throw;
            }

            

        }
    }
}
