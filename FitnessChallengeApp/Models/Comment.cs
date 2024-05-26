using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        
        [Required]
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }

        
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
