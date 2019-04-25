using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithDatebase
{
    public class DatebaseUse:IDisposable
    {
        #region Propertes
        private SqlConnection cnn;
        static readonly Random rand = new Random();
        #endregion

        #region Functions
        public void CreateDateBase(SqlConnection CnnAdd, string DateBaseName)
        {
            string query = $"USE [master]  CREATE DATABASE[{DateBaseName}]; ";
            
            SqlCommand cmd = new SqlCommand(query, CnnAdd);
            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public SqlConnection Connetion(string ConnetionString)
        {
            cnn = new SqlConnection(ConnetionString);
            try
            {
                cnn.Open();
                Console.WriteLine(cnn.ToString());
                Console.WriteLine("Connetion..........");
                return cnn;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void AddTableCategory(SqlConnection CnnAdd, string DateBaseName)
        {
            string query = $@" USE[{DateBaseName}]
                               CREATE TABLE[dbo].[Category](
                               [Id]    UNIQUEIDENTIFIER NOT NULL,
                               [Title] NVARCHAR(MAX)   NOT NULL,
                               CONSTRAINT[PK_Category] PRIMARY KEY CLUSTERED([Id] ASC)
                                                                                      );";
            SqlCommand cmd = new SqlCommand(query, CnnAdd);
            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void AddTableWallet(SqlConnection CnnAdd, string DateBaseName)
        {
            string query = $@"  USE [{DateBaseName}]
                                CREATE TABLE [dbo].[Wallet] (
                                [Id]  UNIQUEIDENTIFIER NOT NULL,
                                [CategoryId]  UNIQUEIDENTIFIER NOT NULL,
                                [Amount]  MONEY     NOT NULL,
                                [Comment]  NVARCHAR (MAX)   NULL,
                                [Day]     DATETIME2 (7)    NOT NULL,
                                [DateCreated] DATETIME2 (7)  
                                CONSTRAINT [DF_Wallet_DateCreated] 
                                DEFAULT (getutcdate()) NOT NULL,
                                CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED ([Id] ASC),
                                CONSTRAINT [FK_Wallet_Category] FOREIGN KEY ([CategoryId])  
                                REFERENCES [dbo].[Category] ([Id]) ON 
                                DELETE CASCADE ON UPDATE CASCADE); ";
            SqlCommand cmd = new SqlCommand(query, CnnAdd);
            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void FinanceManagerDateBase(SqlConnection CnnAdd, string DateBaseName)
        {          
           
            try
            {
                CreateDateBase(CnnAdd, DateBaseName);
                AddTableCategory(CnnAdd, DateBaseName);
                AddTableWallet(CnnAdd, DateBaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }    
        public void FillTableCategoryRandom(SqlConnection CnnAdd, string DateBaseName,string[] CategoryName)
        {           
            string query = $@" USE[{DateBaseName}] 
                               INSERT INTO Category(Id, Title)
                               VALUES(newid(), '{ CategoryName[rand.Next(CategoryName.Length)]}');";  

            SqlCommand cmd = new SqlCommand(query, CnnAdd);
            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void Dispose()
        {
            cnn.Dispose();
        }

        #endregion
    }
}
