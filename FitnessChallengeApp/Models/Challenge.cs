using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Period { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Instructions { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ApplicationUser>? Participants { get; set; }
    }
}
