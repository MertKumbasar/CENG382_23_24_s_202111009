using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


// States for you to try
public class States{

    public string name  { get; set; }
    public int roomNumber  { get; set; }
    public string enter { get; set; }
    public string exit { get; set; }

    public States(string Name,int RoomNumber, string Enter, string Exit){
        name = Name;
        roomNumber = RoomNumber;
        enter = Enter;
        exit = Exit;
    }
    public void displayProperty(){
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Room Number: {roomNumber}");
        Console.WriteLine($"Enterance Day: {enter}");
        Console.WriteLine($"Exit Day: {exit}\n");
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
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string ReserverName { get; set; }
}


public class ReservationHandler
{

    private Reservation[,] reservations;
    private RoomData roomData;

    public ReservationHandler(int days, int numberOfRooms, RoomData roomData_sent)
    {
        reservations = new Reservation[numberOfRooms, days];
        roomData = roomData_sent;
    }

    public void AddReservation(Reservation reservation)
    {

        int startIndex = reservation.Date.Day - 1;
        int numberOfDays = reservation.Time.Day - startIndex;
        int roomIndex = int.Parse(reservation.Room.RoomId) - 1;

        if (startIndex < 0 || startIndex + numberOfDays > reservations.GetLength(1))
        {
            Console.WriteLine("Reservation exceeds the boundaries of the month.");
            return;
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            if (reservations[roomIndex, i] != null)
            {
                Console.WriteLine($"Reservation conflict on {reservation.Date.AddDays(i - startIndex):MM/dd/yyyy}");
                return;
            }
        }

        for (int i = startIndex; i < startIndex + numberOfDays; i++)
        {
            reservations[roomIndex, i] = reservation;
        }

        Console.WriteLine($"Reservation added from {reservation.Date:MM/dd/yyyy} to {reservation.Time:MM/dd/yyyy} for room {reservation.Room.RoomName}.");
    }

    public void DeleteReservation(string reserverName)
    {
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j]?.ReserverName == reserverName)
                {
                    reservations[i, j] = null;
                }
            }
        }

        Console.WriteLine($"\nAll reservations for guest {reserverName} deleted.\n");
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("Weekly Schedule:");
        for (int i = 0; i < reservations.GetLength(0); i++)
        {
            Console.WriteLine($"Room {i + 1} - {roomData.Rooms[i].RoomName}:");
            for (int j = 0; j < reservations.GetLength(1); j++)
            {
                if (reservations[i, j] != null && reservations[i, j] != reservations[i, j + 1])
                {
                    Console.WriteLine($"  {reservations[i, j].Date:dd/MM/yyyy} to {reservations[i, j].Time:dd/MM/yyyy}: {reservations[i, j].ReserverName}");
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
            States state1 = new States("mert", 1, "10/10/2024", "15/10/2024");
            States state2 = new States("sila", 2, "10/9/2024", "15/9/2024");
            States state3 = new States("zeynep", 3, "10/8/2024", "15/8/2024");
            States state4 = new States("tuna", 4, "10/7/2024", "15/7/2024");  
            States selectedState = null;

            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };

            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            ReservationHandler handler = new ReservationHandler(28, roomData.Rooms.Length, roomData);

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

                        int roomIndex = selectedState.roomNumber - 1;

                        DateTime date = DateTime.Parse(selectedState.enter);

                        DateTime time = DateTime.Parse(selectedState.exit);

        
                        Reservation newReservation = new Reservation
                        {
                            Date = date,
                            Time = time,
                            ReserverName = reserverName,
                            Room = roomData.Rooms[roomIndex]
                        };

                        handler.AddReservation(newReservation);

                        break;

                    case 2:
                        Console.Write("\nEnter guest name to delete all reservations: ");
                        string reserverNameToDelete = Console.ReadLine();
                        handler.DeleteReservation(reserverNameToDelete);
                        
                        break;

                    case 3:
                        handler.DisplayWeeklySchedule();
                        break;

                    case 4:
                        programOn = false;
                        Console.WriteLine("Thanks for using.");
                        break;
                    default:
                        Console.WriteLine("Worng input !!");
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
