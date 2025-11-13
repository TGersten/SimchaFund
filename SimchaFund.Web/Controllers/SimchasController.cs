using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;
using System.Diagnostics;

namespace SimchaFund.Web.Controllers
{
    public class SimchasController : Controller
    {
      private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Simchas()
        {
            var repo = new SimchaFundRepo(_connectionString);
            var vm = new SimchasViewModel();

            vm.Simchas = repo.GetAllSimchas();

            return View(vm);
        }

        [HttpPost]
        public IActionResult New(Simcha s)
        {
            var repo = new SimchaFundRepo(_connectionString);
            repo.AddNewSimcha(s);

            return Redirect("/");
        }




    }
}
