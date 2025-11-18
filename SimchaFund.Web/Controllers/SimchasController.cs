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

            if(TempData["message"] != null)
            {
                vm.Message = (string)TempData["message"];
            }

            vm.Simchas = repo.GetAllSimchas();
            vm.TotalContributors = repo.GetContributorCount();

            return View(vm);
        }

        [HttpPost]
        public IActionResult New(Simcha s)
        {
            var repo = new SimchaFundRepo(_connectionString);
            repo.AddNewSimcha(s);

            TempData["message"] = "New Simcha created!";
            return Redirect("/");
        }

        public IActionResult Contributions(string simchaid)
        {
            if (!int.TryParse(simchaid, out int simchaIdValue))
            {
                return Redirect("/");
            }
            var repo = new SimchaFundRepo(_connectionString);
            var vm = new ContributionsViewModel
            {
              
                Simcha = repo.GetSimchaForId(simchaIdValue)
            };

            List<int> ids = repo.GetIdsOfContributorsForASimcha(simchaIdValue);
            vm.Contributors = repo.GetAllContributors();
            foreach(Contributor c in vm.Contributors )
            {
               foreach(int id in ids)
                {
                    if (c.Id == id)
                    {
                        c.Contributed = true;
                    }
                }
            }

            if (vm.Simcha == null)
            {
                return Redirect("/");
            }


            return View(vm);
        }

        [HttpPost]

        public IActionResult UpdateContributions(int simchaId, List<ToInclude> contributors)
        {
            var repo = new SimchaFundRepo(_connectionString);
            repo.DeleteContributionsForSimchaId(simchaId);

            var contributionsToUpdate = contributors.Where(c => c.Include).ToList();
            repo.UpdateContributions(simchaId, contributionsToUpdate);

            TempData["message"] = "Simcha updated successfully!";

            return Redirect("/");
        }

    }
}
