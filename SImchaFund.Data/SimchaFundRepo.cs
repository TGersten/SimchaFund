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

            while (reader.Read())
            {
                simchas.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"]
                });
            }

            foreach (Simcha s in simchas)
            {
                s.AmountOfContributors = GetContributorCountForSimcha(s);
                s.TotalContributed = GetTotalContributedForSimcha(s);
            }
            return simchas;

        }

        public void AddNewSimcha(Simcha s)
        {

            if (s.Name == null || s.Date.ToShortDateString() == "1/1/0001")
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

            foreach (Contributor c in contributors)
            {
                c.TotalContributed = GetTotalContributed(c);
                c.TotalDeposited = GetTotalDeposited(c);
            }


            return contributors;


        }

        private int GetTotalContributed(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(amount) as TotalContributed FROM Contributions WHERE contributorId=@id";
            cmd.Parameters.AddWithValue("@id", c.Id);
            connection.Open();


            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                return 0;

            }
            else
            {
                return Convert.ToInt32(result);
            }

        }
        private int GetTotalContributedForSimcha(Simcha s)
        {
            using var connection = new SqlConnection(_connectionString);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(amount) as TotalContributed FROM Contributions WHERE simchaId=@id";
            cmd.Parameters.AddWithValue("@id", s.Id);
            connection.Open();


            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                return 0;

            }
            else
            {
                return Convert.ToInt32(result);
            }
        }

        private int GetTotalDeposited(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(amount) as TotalDeposited FROM Deposits WHERE contributorId=@id";
            cmd.Parameters.AddWithValue("@id", c.Id);
            connection.Open();


            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                return 0;

            }
            else
            {
                return Convert.ToInt32(result);
            }

        }

        public int GetTotalDepositedForAll()
        {
            using var connection = new SqlConnection(_connectionString);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(amount) as TotalDeposited FROM Deposits ";

            connection.Open();


            return (int)(decimal)cmd.ExecuteScalar();

        }
        public int GetTotaContributedForAll()
        {
            using var connection = new SqlConnection(_connectionString);

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(amount) as TotalContributed FROM Contributions ";

            connection.Open();


            return (int)(decimal)cmd.ExecuteScalar();

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

            if (c.FirstName == null || c.LastName == null || c.CellNumber == null || c.Date.ToShortDateString() == "1/1/0001" || c.InitialDeposit == 0)
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

            return (int)(decimal)cmd.ExecuteScalar();


        }

        public void AddDeposit(Deposit d)
        {
            if (d.ContributorId == 0 || d.Amount == 0 || d.Date.ToShortDateString() == "1/1/0001")
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

        public void EditContributor(Contributor c)
        {
            if (c.FirstName == null || c.LastName == null || c.CellNumber == null || c.Date.ToShortDateString() == "1/1/0001")
            {
                return;
            }

            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Contributors SET FirstName = @firstName, LastName = @lastName, CellNumber= @cellNumber, Date = @date, alwaysInclude = @alwaysInclude WHERE Id = @id";
            cmd.Parameters.AddWithValue("@firstName", c.FirstName);
            cmd.Parameters.AddWithValue("@lastName", c.LastName);
            cmd.Parameters.AddWithValue("@cellNumber", c.CellNumber);
            cmd.Parameters.AddWithValue("@date", c.Date);
            cmd.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            cmd.Parameters.AddWithValue("@id", c.Id);
            connection.Open();

            cmd.ExecuteNonQuery();
        }

        public int GetContributorCount()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT (*) FROM Contributors";
            connection.Open();

            return (int)cmd.ExecuteScalar();
        }

        private int GetContributorCountForSimcha(Simcha s)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT (*) FROM Contributions WHERE SimchaId = @id ";
            cmd.Parameters.AddWithValue("@id", s.Id);

            connection.Open();

            var result = cmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                return 0;

            }
            else
            {
                return Convert.ToInt32(result);
            }
        }


        public Simcha GetSimchaForId(int id)

        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Simchas WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();

            Simcha s = new();

            if (reader.Read())
            {
                s.Id = (int)reader["id"];
                s.Name = (string)reader["name"];
                s.Date = (DateTime)reader["Date"];

            }
            return s;
        }

        public void UpdateContributions(int simchaId, List<ToInclude> contributors)
        {


            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            connection.Open();

            foreach (ToInclude c in contributors)
            {

                cmd.CommandText = "INSERT INTO Contributions (SimchaId, ContributorId, Amount) Values(@simchaId, @contributorId, @amount)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                cmd.Parameters.AddWithValue("@contributorId", c.ContributorId);
                cmd.Parameters.AddWithValue("@amount", c.Amount);

                cmd.ExecuteNonQuery();

            }

        }

        public void DeleteContributionsForSimchaId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Contributions WHERE simchaId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            cmd.ExecuteNonQuery();
        }


        public List<Deposit> GetAllDepositsById(int id)
        {

            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Deposits WHERE ContributorId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            var reader = cmd.ExecuteReader();
            List<Deposit> deposits = new();

            while (reader.Read())
            {
                deposits.Add(new Deposit
                {
                    Id = (int)reader["id"],
                    ContributorId = (int)reader["ContributorId"],
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],


                });
            }

            return deposits;


        }

        public List<Contribution> GetAllContributionsById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributions c JOIN Simchas s on s.id = c.simchaId WHERE c.contributorId= @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            var reader = cmd.ExecuteReader();
            List<Contribution> contributions = new();

            while (reader.Read())
            {
                contributions.Add(new Contribution
                {

                    SimchaName = (string)reader["Name"],
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"],


                });
            }

            return contributions;
        }

        public Contributor GetContributorForId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributors WHERE Id= @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            var reader = cmd.ExecuteReader();

            Contributor c = new();

            if (reader.Read())
            {

                c.Id = (int)reader["Id"];
                c.FirstName = (string)reader["FirstName"];
                c.LastName = (string)reader["LastName"];
                c.CellNumber = (string)reader["CellNumber"];
                c.Date = (DateTime)reader["Date"];
                c.AlwaysInclude = (bool)reader["AlwaysInclude"];

            }

            c.TotalContributed = GetTotalContributed(c);
            c.TotalDeposited = GetTotalDeposited(c);

            return c;


        }
        public List<int> GetIdsOfContributorsForASimcha(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT contributorId From Contributions WHERE simchaId = @simchaId";
            cmd.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();

            var reader = cmd.ExecuteReader();
            List<int> ids= new();

            while (reader.Read())
            {


                int x = (int)reader["contributorId"];
                ids.Add(x);
                   
               
            }

            return ids;
        }
    }

}

