using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class QuotationModel : PageModel
{
    [BindProperty]
    public int Adults { get; set; }

    [BindProperty]
    public int Children { get; set; }

    [BindProperty]
    public int Days { get; set; }

    [BindProperty]
    public int Nights { get; set; }

    [BindProperty]
    public int Rooms { get; set; }
    [BindProperty]
    public int ChildrenWithBed { get; set; }

    [BindProperty]
    public int ChildrenWithoutBed { get; set; }

    [BindProperty]
    public int ExtraBeds { get; set; }

    [BindProperty]
    public string? RoomType { get; set; }

    [BindProperty]
    public decimal TotalRoomAmount { get; set; } // Assuming you calculate and pass the total amount

    [BindProperty]
    public string? SuggestedVehicle { get; set; }
    [BindProperty]
    public decimal RentPerDay { get; set; }
    [BindProperty]
    public decimal TotalRent { get; set; }
    public List<string> Itinerary { get; set;  }
    public decimal GrandTotal { get; set; }

    public QuotationModel()
    {
        Itinerary = new List<string>();
    }
    public void OnPost()
    {
        Itinerary.Add("Day 1: Arrival in Singapore - Transfer to hotel and leisure time");
        Itinerary.Add("Day 2:Singapore City Tour - Visit Marina Bay Sands, Merlion Park, Chinatown.");
        Itinerary.Add("Day 3:Sentosa Island Tour - Explore Universal Studios, S.E.A. Aquarium, and more");
        Itinerary.Add("Day 4: Free Day for Shopping and Local Exploration.");
        Itinerary.Add("Day 5: Visit Chinatown,Chinatown Heritage Centre and Fort Canning Park.");

        if (string.IsNullOrEmpty(RoomType) || RoomType == "Select")
        {
            ModelState.AddModelError("RoomType", "Please select a valid room type.");
            return;
        }

        // Room rent logic
        decimal roomPricePerNight = GetRoomPrice(RoomType);
        decimal pricePerChildWithBed = 150; // Assuming 150 INR per child with a bed
        decimal pricePerExtraBed = 150; // Assuming 150 INR per extra bed

        // Calculate the total room cost (with children and extra beds)
        TotalRoomAmount = (Rooms * roomPricePerNight * Nights) +
                          (ChildrenWithBed * pricePerChildWithBed * Nights) +
                          (ExtraBeds * pricePerExtraBed * Nights);

        // Vehicle rent is already passed in the form and set directly
        // TotalVehicleRent comes from the hidden form field in the front-end
        // Transportation logic
        CalculateTransportation();

        // Calculate the Grand Total
        GrandTotal = TotalRoomAmount + TotalRent;
        // Now, the TotalAmount is automatically calculated using the TotalAmount property (TotalRoomAmount + TotalVehicleRent)
    }

    // Helper method to determine the price per room based on the selected room type
    private static decimal GetRoomPrice(string roomType)
    {
        switch (roomType)
        {
            case "Superior":
                return 1500; // I+  `NR 1500 for Superior Room
            case "Deluxe":
                return 2000; // INR 2000 for Deluxe Room
            case "Deluxe prem":
                return 2500; // INR 2500 for Deluxe Premiere Room
            case "Fam suite":
                return 3000; // INR 3000 for Family Suite
            default:
                return 0; // Default case for invalid room type
        }
    }
    private void CalculateTransportation()
    {
        int totalPeople = Adults + Children;

        // Determine vehicle and rent based on total number of people
        if (totalPeople <= 4)
        {
            SuggestedVehicle = "Car";
            RentPerDay = 500; // Rent per day for Car
        }
        else if (totalPeople <= 7)
        {
            SuggestedVehicle = "Mini Van";
            RentPerDay = 1000; // Rent per day for Mini Van
        }
        else
        {
            SuggestedVehicle = "Bus";
            RentPerDay = 1500; // Rent per day for Bus
        }

        // Calculate total rent based on the number of days
        TotalRent = RentPerDay * Days;
    }
}
