using Microsoft.AspNetCore.Mvc;

public class TaskController : Controller
{
    private string connectionString = ConfigurationManager.ConnectionStrings["CRMConnectDb"].ConnectionString;

    // Get all tasks
    public ActionResult Index()
    {
        List<Task> taskList = new List<Task>();

        using (var connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Tasks";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                taskList.Add(new Task
                {
                    TaskID = reader.GetInt32(0),
                    AssignedTo = reader.GetString(1),
                    TaskDescription = reader.GetString(2),
                    DueDate = reader.GetDateTime(3),
                    Status = reader.GetString(4),
                    SalesID = reader.GetInt32(5)
                });
            }
        }

        return View(taskList);
    }

    // Create a new task
    [HttpPost]
    public ActionResult Create(Task task)
    {
        using (var connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Tasks (AssignedTo, TaskDescription, DueDate, Status, SalesID) VALUES (:AssignedTo, :TaskDescription, :DueDate, :Status, :SalesID)";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter(":AssignedTo", task.AssignedTo));
            cmd.Parameters.Add(new OracleParameter(":TaskDescription", task.TaskDescription));
            cmd.Parameters.Add(new OracleParameter(":DueDate", task.DueDate));
            cmd.Parameters.Add(new OracleParameter(":Status", task.Status));
            cmd.Parameters.Add(new OracleParameter(":SalesID", task.SalesID));

            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Index");
    }
}

