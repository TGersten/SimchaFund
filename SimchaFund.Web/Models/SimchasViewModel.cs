using SimchaFund.Data;
using System.Numerics;

namespace SimchaFund.Web.Models
{
    public class SimchasViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int TotalContributors { get; set; }
        public int ContributorsForSimcha { get; set; }

    }
}
