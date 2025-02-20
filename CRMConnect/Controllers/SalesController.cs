using Microsoft.AspNetCore.Mvc;

public class SalesController : Controller
{
    private string connectionString = ConfigurationManager.ConnectionStrings["CRMConnectDb"].ConnectionString;

    // Get all sales
    public ActionResult Index()
    {
        List<Sales> salesList = new List<Sales>();

        using (var connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Sales";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                salesList.Add(new Sales
                {
                    SalesID = reader.GetInt32(0),
                    CustomerID = reader.GetInt32(1),
                    SalesDate = reader.GetDateTime(2),
                    SalesAmount = reader.GetDecimal(3),
                    SalesStatus = reader.GetString(4)
                });
            }
        }

        return View(salesList);
    }

    // Add a new sale
    [HttpPost]
    public ActionResult Create(Sales sale)
    {
        using (var connection = new OracleConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Sales (CustomerID, SalesDate, SalesAmount, SalesStatus) VALUES (:CustomerID, :SalesDate, :SalesAmount, :SalesStatus)";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter(":CustomerID", sale.CustomerID));
            cmd.Parameters.Add(new OracleParameter(":SalesDate", sale.SalesDate));
            cmd.Parameters.Add(new OracleParameter(":SalesAmount", sale.SalesAmount));
            cmd.Parameters.Add(new OracleParameter(":SalesStatus", sale.SalesStatus));

            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Index");
    }
}
