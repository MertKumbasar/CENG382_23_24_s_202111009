using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;


namespace MyApp.Namespace
{
    [Authorize]
    public class CreateReservationModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;
        

        public CreateReservationModel(WebAppDataBaseContext context)
        {
            _context = context;
            
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation();

        public List<Room> Rooms { get; set; }

        public void OnGet()
        {
            Rooms = _context.Rooms.ToList();

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Reservation.DateTime.Minute != 0 || Reservation.DateTime.Second != 0)
            {
                ModelState.AddModelError("Reservation.DateTime", "Please select a time!!");
                return Page();
            }

            bool isConflict = _context.Reservations
                .Any(r => r.DateTime == Reservation.DateTime &&
                          !r.IsDeleted);

            if (isConflict)
            {
                ModelState.AddModelError("Reservation.DateTime", "A reservation for this room at the same date and time already exists (Conflict)!!");
                Rooms = _context.Rooms.ToList(); 
                return Page();
            }

            Reservation.ReservedBy = User.Identity.Name; 
            _context.Reservations.Add(Reservation);
            _context.SaveChanges();

            
            var log = new logger
            {
                Timestamp = DateTime.Now,
                ReservationId = Reservation.Id,
                RoomId = Reservation.RoomId,
                UserId = Reservation.ReservedBy,
                ReservationDate = Reservation.DateTime
            };

            _context.loggers.Add(log);
            _context.SaveChanges();

            return RedirectToPage("/DisplayReservation");
        }
    }
}
