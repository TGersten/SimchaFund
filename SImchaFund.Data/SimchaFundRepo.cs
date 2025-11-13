using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
    public class SimchaFundRepo
    {
        private string _connectionString;

        public SimchaFundRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Simcha> GetAllSimchas()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Simchas ORDER BY Date ASC";
            connection.Open();

            var simchas = new List<Simcha>();
            var reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                simchas.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"]
                });
            }

            return simchas;

        }

        public void AddNewSimcha(Simcha s)
        {
           
            if(s.Name == null || s.Date.ToShortDateString() == "1/1/0001")
            {
                return;
            }
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Simchas (Name, Date) Values(@name, @date)";
            cmd.Parameters.AddWithValue("@name", s.Name);
            cmd.Parameters.AddWithValue("@date", s.Date);
            connection.Open();

            cmd.ExecuteNonQuery();


        }

        public List<Contributor> GetAllContributors()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributors ";
            connection.Open();

            var contributors = new List<Contributor>();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    CellNumber = (string)reader["CellNumber"],
                    Date = (DateTime)reader["Date"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"]
                });
            }

            return contributors;


        }
        public void AddInitialDeposit(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits (ContributorId, Amount, Date) Values (@contributorId, @amount, @date)";
            cmd.Parameters.AddWithValue("@contributorId", c.Id);
            cmd.Parameters.AddWithValue("@amount", c.InitialDeposit);
            cmd.Parameters.AddWithValue("@date", c.Date);
            connection.Open();

            cmd.ExecuteNonQuery();
        }
        public int AddNewContributor(Contributor c)
        {

            if (c.FirstName == null || c.LastName== null || c.CellNumber == null || c.Date.ToShortDateString() == "1/1/0001" || c.InitialDeposit == 0)
            {
                return 0;
            }

           
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Contributors ( FirstName, LastName, CellNumber, Date, ALwaysInclude) Values(@firstName, @lastName, @cellNumber, @date, @alwaysInclude) SELECT SCOPE_IDENTITY()";
            
            cmd.Parameters.AddWithValue("@firstName", c.FirstName);
            cmd.Parameters.AddWithValue("@lastName", c.LastName);
            cmd.Parameters.AddWithValue("@cellNumber", c.CellNumber);
            cmd.Parameters.AddWithValue("@date", c.Date);
            cmd.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            connection.Open();

            return (int)(decimal) cmd.ExecuteScalar();
            

        }

        public void AddDeposit(Deposit d)
        {
            if(d.ContributorId == 0 || d.Amount == 0 || d.Date.ToShortDateString() == "1/1/0001")
            {
                return;
            }
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits ( ContributorId, Amount, Date) Values(@contributorId, @amount, @date)";

            cmd.Parameters.AddWithValue("@contributorId", d.ContributorId);
            cmd.Parameters.AddWithValue("@amount", d.Amount);
            cmd.Parameters.AddWithValue("@date", d.Date);
          
            connection.Open();
            cmd.ExecuteNonQuery();

        }
    }
}
