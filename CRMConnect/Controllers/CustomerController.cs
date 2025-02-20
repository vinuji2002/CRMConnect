using System.Web.Mvc;
using CRMConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRMConnect.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository customerRepo = new CustomerRepository();

        public ActionResult Index()
        {
            var customers = customerRepo.GetCustomers();
            return View(customers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            customerRepo.AddCustomer(customer);
            return RedirectToAction("Index");
        }
    }
}
