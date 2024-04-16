using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

// States for you to try
public class States
{
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
        Console.WriteLine($"Exit Day: {enterHour}\n");
    }

}

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; }
}

public class Room
{
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    [JsonPropertyName("roomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}

public class Reservation
{
    public Room Room { get; set; }
    public string Day { get; set; }
    public DateTime Time { get; set; }
    public string ReserverName { get; set; }
}

public class ReservationHandler
{
    private Dictionary<string, Dictionary<Room, List<(DateTime, string)>>> weeklyReservations;
    private TimeSpan breakTime = TimeSpan.FromMinutes(40);

    public ReservationHandler(RoomData roomData)
    {
        weeklyReservations = new Dictionary<string, Dictionary<Room, List<(DateTime, string)>>>();

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            weeklyReservations[day.ToString()] = new Dictionary<Room, List<(DateTime, string)>>();
        }

        foreach (Room room in roomData.Rooms)
        {
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek day = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
                weeklyReservations[day.ToString()][room] = new List<(DateTime, string)>();
            }
        }
    }

    public void AddReservation(string day, string roomNumber, string reserverName, DateTime enterTime)
    {
        Room room = Array.Find(weeklyReservations[day].Keys.ToArray(), r => r.RoomId == roomNumber);
        List<(DateTime, string)> reservations = weeklyReservations[day][room];

        DateTime endTime = enterTime.AddMinutes(40); 

        if (reservations.Any(reservation => enterTime < reservation.Item1.Add(breakTime) && endTime > reservation.Item1))
        {
            Console.WriteLine("There is a reservation conflict. Please choose another time.");
            return;
        }

        reservations.Add((enterTime, reserverName));
        Console.WriteLine($"Reservation added for room {roomNumber} on {day} at {enterTime:hh:mm tt}.");
    }

    public void PrintWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule:");

        
        for (int i = 0; i < 7; i++)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
            string dayOfWeekString = dayOfWeek.ToString();

            Console.WriteLine($"Day: {dayOfWeekString}");

            foreach (var roomKvp in weeklyReservations[dayOfWeekString])
            {
                Room room = roomKvp.Key;
                List<(DateTime, string)> reservations = roomKvp.Value;

                if (reservations.Count == 0)
                {
                    continue;
                }

                Console.WriteLine($"Room {room.RoomId} ({room.RoomName}):");

                foreach ((DateTime time, string reserverName) in reservations)
                {
                    Console.WriteLine($"  {time:hh:mm tt} - {reserverName}");
                }
            }

            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";

        try
        {
            States state1 = new States("Mert", "001", "Monday", "11:00");
            States state2 = new States("Sila", "001", "Monday", "11:20");
            States state3 = new States("Zeynep", "002", "Monday", "11:20");
            States state4 = new States("Tuna", "003", "Friday", "11:00");
            States selectedState = null;

            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            ReservationHandler handler = new ReservationHandler(roomData);

            bool programOn = true;
            while (programOn)
            {
                Console.WriteLine("To add a new reservation press 1.");
                Console.WriteLine("To delete reservation press 2.");
                Console.WriteLine("To display weekly schedule press 3.");
                Console.WriteLine("To exit press 4.");

                int selection = int.Parse(Console.ReadLine());

                switch (selection)
                {
                    case 1:

                        Console.WriteLine("Select state 1:");
                        state1.displayProperty();
                        Console.WriteLine("Select state 2:");
                        state2.displayProperty();
                        Console.WriteLine("Select state 3:");
                        state3.displayProperty();
                        Console.WriteLine("Select state 4:");
                        state4.displayProperty();

                        int selection2 = int.Parse(Console.ReadLine());

                        switch (selection2)
                        {
                            case 1:
                                selectedState = state1;
                                break;
                            case 2:
                                selectedState = state2;
                                break;
                            case 3:
                                selectedState = state3;
                                break;
                            case 4:
                                selectedState = state4;
                                break;
                            default:
                                Console.WriteLine("Invalid input !!");
                                break;
                        }


                        string reserverName = selectedState.name;
                        string roomNumber = selectedState.roomNumber;
                        string day = selectedState.enterDay;
                        DateTime time = DateTime.Parse(selectedState.enterHour);

                        handler.AddReservation(day, roomNumber, reserverName, time);
                        break;

                    case 2:
                        Console.Write("\nEnter guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();
                        break;

                    case 3:
                        handler.PrintWeeklySchedule();
                        break;

                    case 4:
                        programOn = false;
                        Console.WriteLine("Thanks for using.");
                        break;
                    default:
                        Console.WriteLine("Wrong input !!");
                        break;
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File '{jsonFilePath}' not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Error deserializing JSON data from file '{jsonFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
