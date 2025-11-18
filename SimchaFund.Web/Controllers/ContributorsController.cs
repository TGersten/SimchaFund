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

            if (TempData["message"]!=null)
            {
                vm.Message = (string)TempData["message"];
            }
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

            TempData["message"] = "New Contributor created!";
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
            TempData["message"] = "Contributor updated successfully!";
            return Redirect("/contributors/index");
        }

        public IActionResult History(string contribid)
        {
            var repo = new SimchaFundRepo(_connectionString);
            var vm = new HistoryViewModel();

            if (!int.TryParse(contribid, out int contribIdValue))
            {
                return Redirect("/");
            }

            var Deposits = repo.GetAllDepositsById(contribIdValue);
            var Contributions = repo.GetAllContributionsById(contribIdValue);

            List<Transaction> transactions = new();

            foreach (Deposit d in Deposits)
            {
                transactions.Add(new()
                {
                    Action = "Deposit",
                    Date = d.Date,
                    Amount = d.Amount
                });


            }

            foreach (Contribution c in Contributions)
            {
                transactions.Add(new()
                {
                    Action = $"Contribution for {c.SimchaName} simcha",
                    Date = c.Date,
                    Amount = c.Amount
                });

            }

            vm.Transactions = transactions.OrderByDescending(t => t.Date).ToList();
            vm.Contributor = repo.GetContributorForId(contribIdValue);

            return View(vm);
        }


    }

}
