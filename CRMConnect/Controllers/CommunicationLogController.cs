using CRMConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace CRMConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationLogController : ControllerBase
    {
        private readonly string connectionString = "User Id=crmconnect;Password=password;Data Source=YourOracleDB;";

        [HttpGet]
        public IActionResult GetAllLogs()
        {
            List<CommunicationLog> logs = new List<CommunicationLog>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM CommunicationLog";
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(new CommunicationLog
                    {
                        LogID = Convert.ToInt32(reader["LogID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CommunicationType = reader["CommunicationType"].ToString(),
                        CommunicationDate = Convert.ToDateTime(reader["CommunicationDate"]),
                        Notes = reader["Notes"]?.ToString()
                    });
                }
            }
            return Ok(logs);
        }

        [HttpPost]
        public IActionResult CreateLog([FromBody] CommunicationLog log)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO CommunicationLog (CustomerID, CommunicationType, CommunicationDate, Notes) " +
                               "VALUES (:CustomerID, :CommunicationType, :CommunicationDate, :Notes)";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(":CustomerID", log.CustomerID);
                cmd.Parameters.Add(":CommunicationType", log.CommunicationType);
                cmd.Parameters.Add(":CommunicationDate", log.CommunicationDate);
                cmd.Parameters.Add(":Notes", log.Notes);

                cmd.ExecuteNonQuery();
            }
            return Ok(new { Message = "Log added successfully" });
        }
    }
}
