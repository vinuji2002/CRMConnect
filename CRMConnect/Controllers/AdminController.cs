using CRMConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace CRMConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly string connectionString = "User Id=crmconnect;Password=password;Data Source=YourOracleDB;";

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            List<User> users = new List<User>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Users";
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = int.Parse(reader["UserID"].ToString()),
                        Username = reader["Username"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }
            return Ok(users);
        }

        [HttpPost("addUser")]
        public IActionResult AddUser([FromBody] User user)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (:Username, :PasswordHash, :Role)";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":Username", user.Username);
                cmd.Parameters.Add(":PasswordHash", user.PasswordHash);
                cmd.Parameters.Add(":Role", user.Role);

                cmd.ExecuteNonQuery();
            }
            return Ok(new { Message = "User added successfully" });
        }
    }
}

