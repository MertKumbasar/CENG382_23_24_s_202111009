using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Challenge> Challenges { get; set; }
        public string SearchString { get; set; }

        public async Task OnGetAsync(string searchString)
        {
            var challenges = from c in _context.Challenges
                             select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                challenges = challenges.Where(s => s.Category.Contains(searchString));
            }

            Challenges = await challenges.ToListAsync();
        }
    }
}
