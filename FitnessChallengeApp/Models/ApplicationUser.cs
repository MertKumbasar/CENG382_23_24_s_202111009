using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FitnessChallengeApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public ICollection<Challenge>? FavoriteChallenges { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
