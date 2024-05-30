using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using loginDemo.Models;
using loginDemo.Data;

namespace MyApp.Namespace
{
    [Authorize]
    public class CreateRoomModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;

        public CreateRoomModel(WebAppDataBaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room NewRoom { get; set; } = new Room(); 

        public void OnGet(){
        
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewRoom == null)
            {
                return Page();
            }

            if (_context.Rooms.Any(r => r.RoomName == NewRoom.RoomName))
            {
                ModelState.AddModelError("NewRoom.RoomName", "A room with this name already exists.");
                return Page();
            }

            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();

            var log = new logger
            {
                Timestamp = DateTime.Now,
                RoomId = NewRoom.Id,
                UserId = User.Identity.Name
            };

            _context.loggers.Add(log);
            _context.SaveChanges();

            return RedirectToPage("/DisplayRoom");
        }
    }
}
