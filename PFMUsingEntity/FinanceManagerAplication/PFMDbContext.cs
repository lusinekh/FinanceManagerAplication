using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagerAplication
{
    public  class PFMDbContext: DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=DESKTOP-I87ABNR\SQLEXPRESS;Database=PFM_EFCore;Integrated Security=True");
            optionsBuilder.UseSqlServer(
                @"Server=DESKTOP-I87ABNR\SQLEXPRESS;Database=PFM_EFCore16;Integrated Security=True");
        }

    }
}
