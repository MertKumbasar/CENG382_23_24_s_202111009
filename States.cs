// This is state class which lets you try possible inputs without typing them all just choose the state you want to enter 
// and you can add more states if you like from program.cs.

public record States{
    public string name { get; set; }
    public string roomNumber { get; set; }
    public string enterDay { get; set; }
    public string enterHour { get; set; }

    public States(string Name, string RoomNumber, string Enter, string Exit)
    {
        name = Name;
        roomNumber = RoomNumber;
        enterDay = Enter;
        enterHour = Exit;
    }
    public void displayProperty()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Room Number: {roomNumber}");
        Console.WriteLine($"Enterance Day: {enterDay}");
        Console.WriteLine($"Enterance Time: {enterHour}\n");
    }
}