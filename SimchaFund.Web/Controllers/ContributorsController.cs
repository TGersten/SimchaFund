using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;
using System.Security.Cryptography.Pkcs;

namespace SimchaFund.Web.Controllers
{
    public class ContributorsController : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Index()
        {
            var repo = new SimchaFundRepo(_connectionString);
            var vm = new ContributorsViewModel();
            int totalDeposited = repo.GetTotalDepositedForAll();
            int totalContributed = repo.GetTotaContributedForAll();

            vm.Total = totalDeposited - totalContributed;

            vm.Contributors = repo.GetAllContributors();
           
            return View(vm);
        }

        [HttpPost]
        public IActionResult New(Contributor c)
        {
            var repo = new SimchaFundRepo(_connectionString);
            int newId = repo.AddNewContributor(c);
            c.Id = newId;
            if (c.Id != 0)
            {
                repo.AddInitialDeposit(c);
            }
            return Redirect("/Contributors/Index");

        }

        [HttpPost]
        public IActionResult Deposit(Deposit d)
        {
            var repo = new SimchaFundRepo(_connectionString);
            repo.AddDeposit(d);

            return Redirect("/contributors/Index");
        }

        [HttpPost]
        public IActionResult Edit(Contributor c)
        {

            var repo = new SimchaFundRepo(_connectionString);
            repo.EditContributor(c);
            return Redirect("/contributors/index");
        }


    }
    
}
