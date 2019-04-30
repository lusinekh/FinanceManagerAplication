using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerDatebase
{
    public class PersonalFinanceManagment : IDisposable
    {
        #region Properties
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        private static Random rand = new Random();
        private SqlTransaction transaction;
        #endregion

        #region Constructors
        public PersonalFinanceManagment(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }
        #endregion

        #region Functions
        public void Dispose()
        {
            _connection.Dispose();
        }
        public async Task CreateDbAsync(string dbName)
        {
            try
            {
                string query = $"USE [master]  CREATE DATABASE[{dbName}]; ";
                await _connection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("Connetion..........");
                }
                _connection.Close();
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CreateCategoryTableAsync(string dbName)
        {
            string query = $@" USE[{dbName}]
                               CREATE TABLE[dbo].[Category](
                               [Id]    UNIQUEIDENTIFIER NOT NULL,
                               [Title] NVARCHAR(MAX)   NOT NULL,
                               [IsDebit] [bit] NOT NULL
                               CONSTRAINT[PK_Category] PRIMARY KEY CLUSTERED([Id] ASC)
                                                                                      );";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                _connection.Close();
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CreateWalletTableAsync(string dbName)
        {
            string query = $@"  USE [{dbName}]
                                CREATE TABLE [dbo].[Wallet] (
                                [Id]  UNIQUEIDENTIFIER NOT NULL,
                                [CategoryId]  UNIQUEIDENTIFIER NOT NULL,
                                [Amount]  MONEY     NOT NULL,
                                [Comment]  NVARCHAR (MAX)   NULL,
                                [Day1]     DATETIME2 (7)    NOT NULL,
                                [DateCreated] DATETIME2 (7)  
                                CONSTRAINT [DF_Wallet_DateCreated] 
                                DEFAULT (getutcdate()) NOT NULL,
                                CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED ([Id] ASC),
                                CONSTRAINT [FK_Wallet_Category] FOREIGN KEY ([CategoryId])  
                                REFERENCES [dbo].[Category] ([Id]) ON 
                                DELETE CASCADE ON UPDATE CASCADE); ";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task FinanceManagerDatebaseAsync(string dbName)
        {
            try

            {
                await CreateDbAsync(dbName);
                await CreateCategoryTableAsync(dbName);
                await CreateWalletTableAsync(dbName);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task InitCategoryTableRandom(string dbName, string[] CategoryName)
        {
            int Db = rand.Next(0, 2);

            string query = $@" USE[{dbName}] 
                               INSERT INTO  Category(Id,Title,IsDebit)
                               values(newid(),'{ CategoryName[rand.Next(CategoryName.Length)]}',{Db} );";
            try
            {

                await _connection.OpenAsync();
                transaction = _connection.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                    transaction.Commit();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {

                Console.WriteLine(e);

            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InitCategoryTable(string dbName, string CategoryName, bool Db)
        {
            int Out = Db == true ? 1 : 0;

            string query = $@" USE[{dbName}] 
                               INSERT INTO  Category(Id,Title,IsDebit)
                               values(newid(),'{ CategoryName}',{Out} );";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string[] GateCategoryIdArray(string DateBaseName)
        {

            _connection.Open();

            StringBuilder itms = new StringBuilder();

            string query = $@" USE[{DateBaseName}] 
                               select Id
                               from Category
                                          ";
            SqlCommand commandShow = new SqlCommand(query, _connection);
            try
            {
                SqlDataReader reader = commandShow.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string itm = reader["Id"].ToString();
                        itms.Append(itm);
                    }
                }
                reader.Close();
                _connection.Close();
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }

            return itms.ToString().Split(' ').ToArray();
        }
        public async Task InitTableWalletRandom(string dbName)
        {
            decimal amount = rand.Next(1000, 100000000);
            int year = rand.Next(1999, 2019);
            int month = rand.Next(1, 12);
            int day = rand.Next(1, 28);
            string dayfinish = $"{year}-{month}-{day}";
            string datecreated = $"{DateTime.Now}";

            var CategoryIdArray = GateCategoryIdArray(dbName);

            string query = $@" USE[{dbName}] 
                                  INSERT INTO Wallet (Id, CategoryId, Amount,Day1,DateCreated)
                                  VALUES (newid(), 
                                  '{ CategoryIdArray[rand.Next(CategoryIdArray.Length)]}' ,
                                  {amount},'{dayfinish}' ,'{datecreated}');";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InsertWalletTable(string dbName, string CategoryId, decimal amount, string comment, DateTime data)
        {
            string query = $@" USE[{dbName}] 
                               INSERT INTO Wallet (Id, CategoryId, Amount,Day1,DateCreated)
                               VALUES (newid(), 
                               '{CategoryId}' ,
                               {amount},'{comment}','{data}' ,'{DateTime.Now}');";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal AmountCalculate(string dbName, DateTime start, DateTime end)
        {
            _connection.Open();
            decimal amount = 0;

            string query = $@"USE[{dbName}]        
                            SELECT Day1, IsDebit, Sum(
                            case when IsDebit = 1 then[Amount] else -[Amount] end
                            ) As SumAmount
                            FROM Wallet
                            LEFT JOIN Category
                            ON Wallet.CategoryId = Category.Id
                            where Day1 BETWEEN '{start}' AND '{end}'
                            Group by  Day1,IsDebit";

            SqlCommand commandShow = new SqlCommand(query, _connection);
            try
            {
                SqlDataReader reader = commandShow.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        amount = (decimal)reader["SumAmount"];

                    }
                }
                reader.Close();
                _connection.Close();
                return amount;
            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task DeleteAllItms(string dbName, string tableName)
        {
            string query = $@"USE[{dbName}]  DELETE FROM {tableName}";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteItmById(string dbName, string tableName, string Id)
        {
            string query = $@"USE[{dbName}]  DELETE FROM {tableName}
                              WHERE Id='{Id}';";
            try
            {

                await _connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                _connection.Close();

            }

            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}



