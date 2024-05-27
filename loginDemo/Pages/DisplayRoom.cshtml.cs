using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using loginDemo.Models;
using loginDemo.Data;

namespace MyApp.Namespace
{
    public class DisplayRoomModel : PageModel
    {
        private readonly WebAppDataBaseContext _context;
        public DisplayRoomModel(WebAppDataBaseContext context)
        {
            _context = context;
        }
        public List <Room> NewRoomList { get; set; } = new List<Room>();
        public void OnGet()
        {
            NewRoomList = _context.Rooms.ToList();
        }
    }
}
