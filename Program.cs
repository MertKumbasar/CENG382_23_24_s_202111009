using System.Text.Json;
using System.Text.Json.Serialization;

// In Lab 6, my code didn't satisfy the Single Responsibility Principle and Dependency Injection Principles. 
// The reason behind that is a class needs more classes to accomplish its tasks. Additional classes had more than one single task, which is not compatible with 
// single responsibility.






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
            States state5 = new States("Ibrahim", "006", "Sunday", "15:00");
            States selectedState = null;

            RoomHandler roomHandler = new RoomHandler(jsonFilePath);
            var roomData = roomHandler.GetRooms();

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
                        Console.WriteLine("Select state 5:");
                        state5.displayProperty();

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
                            case 5:
                                selectedState = state5;
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
                        handler.DeleteReservationByName(reserverNameToDelete);
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