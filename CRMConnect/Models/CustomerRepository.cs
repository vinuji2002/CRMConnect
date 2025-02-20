using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CRMConnect.Models
{
    public class CustomerRepository
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;

        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT * FROM Customers", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Address = reader.GetString(4)
                        });
                    }
                }
            }
            return customers;
        }

        public void AddCustomer(Customer customer)
        {
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("INSERT INTO Customers (Name, Email, Phone, Address) VALUES (:Name, :Email, :Phone, :Address)", conn);
                cmd.Parameters.Add("Name", customer.Name);
                cmd.Parameters.Add("Email", customer.Email);
                cmd.Parameters.Add("Phone", customer.Phone);
                cmd.Parameters.Add("Address", customer.Address);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
