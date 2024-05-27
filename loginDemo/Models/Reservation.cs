using System;
using System.Collections.Generic;

namespace loginDemo.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int? Room { get; set; }

    public DateTime DateTime { get; set; }

    public string? ReservedBy { get; set; }
}
