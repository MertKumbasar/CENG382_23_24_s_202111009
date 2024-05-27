using System;
using System.Collections.Generic;

namespace loginDemo.Models;

public partial class Room
{
    public int Id { get; set; }

    public string RoomName { get; set; } = null!;

    public int Capacity { get; set; }
}
