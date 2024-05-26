using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace FitnessChallengeApp.Pages.Challenges
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Challenge Challenge { get; set; }
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Comment NewComment { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges
                .Include(c => c.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Challenge == null)
            {
                return NotFound();
            }

            Comments = Challenge.Comments.ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            NewComment.ChallengeId = id;
            NewComment.UserId = userId;
            NewComment.CreatedAt = DateTime.Now;

            // Clear the navigation properties to avoid validation issues
            NewComment.User = null;
            NewComment.Challenge = null;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        _logger.LogWarning($"Error in {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                Challenge = await _context.Challenges
                    .Include(c => c.Comments)
                    .ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

                Comments = Challenge.Comments?.ToList() ?? new List<Comment>();
                return Page();
            }

            try
            {
                _context.Comments.Add(NewComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving comment");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the comment.");
                Comments = Challenge.Comments?.ToList() ?? new List<Comment>();
                return Page();
            }

            return RedirectToPage(new { id });
        }
    }
}
