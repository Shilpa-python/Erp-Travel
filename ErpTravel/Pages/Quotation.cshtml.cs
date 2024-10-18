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
    public int Nights { get; set; }

    [BindProperty]
    public int Days { get; set; } // This will hold the value for Days (Nights + 1)

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
    public decimal TotalRoomAmount { get; set; }

    [BindProperty]
    public string? SuggestedVehicle { get; set; }
    
    [BindProperty]
    public decimal RentPerDay { get; set; }
    
    [BindProperty]
    public decimal TotalRent { get; set; }

    public List<string> Itinerary { get; set; }
    public decimal GrandTotal { get; set; }
    public string? SelectedItinerary { get; set; }

    public decimal PerHeadAdultCost { get; set; }
    public decimal PerHeadChildWithBedCost { get; set; }
    public decimal PerHeadChildWithoutBedCost { get; set; }
    public decimal PerHeadExtraBedCost { get; set; }
    public QuotationModel()
    {
        Itinerary = new List<string>();
    }

    public void OnGet()
    {
        // Retrieve the selected itinerary from TempData
        SelectedItinerary = TempData["SelectedItinerary"] as string;

        // If there is a selected itinerary, generate it
        if (!string.IsNullOrEmpty(SelectedItinerary))
        {
            if (SelectedItinerary == "Itinerary1")
            {
                Itinerary.Add("Day 1: Arrival and City Tour.");
                Itinerary.Add("Day 2: Cultural visits and Museums.");
                Itinerary.Add("Day 3: Departure.");
            }
            else if (SelectedItinerary == "Itinerary2")
            {
                Itinerary.Add("Day 1: Adventure Trek.");
                Itinerary.Add("Day 2: Nature walk and Safari.");
                Itinerary.Add("Day 3: Departure.");
            }
            else if (SelectedItinerary == "Itinerary3")
            {
                Itinerary.Add("Day 1: Relax on the Beach.");
                Itinerary.Add("Day 2: Explore the Coastal town.");
                Itinerary.Add("Day 3: Departure.");
            }
        }
    }
    public void OnPost(string selectedItinerary)
    {
        // Ensure Days are correctly calculated from Nights
        Days = Nights + 1;

        // Call itinerary generation and vehicle rent calculations
        GenerateItinerary(Days);  // Use Days instead of Nights
        CalculateTransportation(Days); // Use Days instead of Nights
        if (selectedItinerary == "Itinerary1")
        {
            Itinerary.Add("Day 1: Arrival and City Tour.");
            Itinerary.Add("Day 2: Cultural visits and Museums.");
            Itinerary.Add("Day 3: Departure.");
        }
        else if (selectedItinerary == "Itinerary2")
        {
            Itinerary.Add("Day 1: Adventure Trek.");
            Itinerary.Add("Day 2: Nature walk and Safari.");
            Itinerary.Add("Day 3: Departure.");
        }
        else if (selectedItinerary == "Itinerary3")
        {
            Itinerary.Add("Day 1: Relax on the Beach.");
            Itinerary.Add("Day 2: Explore the Coastal town.");
            Itinerary.Add("Day 3: Departure.");
        }

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
                          (ChildrenWithBed * pricePerChildWithBed) +
                          (ExtraBeds * pricePerExtraBed);

        

        // Calculate the Grand Total
        GrandTotal = TotalRoomAmount + TotalRent;

       
       

        // Calculate the total room cost (with children and extra beds)
        decimal totalAdultRoomCost = Rooms * roomPricePerNight * Nights;
        decimal totalChildWithBedCost = ChildrenWithBed * pricePerChildWithBed;
        decimal totalExtraBedCost = ExtraBeds * pricePerExtraBed;
        TotalRoomAmount = totalAdultRoomCost + totalChildWithBedCost + totalExtraBedCost;

        // Calculate the Grand Total
        GrandTotal = TotalRoomAmount + TotalRent;

        // Per-head cost breakdown
        if (Adults > 0)
        {
            // Adult room cost per head
            PerHeadAdultCost = totalAdultRoomCost / Adults;
        }

        if (ChildrenWithBed > 0)
        {
            // Child with bed cost per head
            PerHeadChildWithBedCost = totalChildWithBedCost / ChildrenWithBed;
        }

        if (ChildrenWithoutBed > 0)
        {
            // For children without beds, assume they don't add to room cost, so their per-head cost is zero.
            PerHeadChildWithoutBedCost = 0; // Or any fixed cost if applicable
        }

        if (ExtraBeds > 0)
        {
            // Extra bed cost per head
            PerHeadExtraBedCost = totalExtraBedCost / ExtraBeds;
        }


    }

    // Auto-generate itinerary based on the number of days
    private void GenerateItinerary(int days)
    {
        Itinerary.Clear(); // Clear any existing itinerary

        var activities = new List<string>
    {
        "Arrival and transfer to the hotel.",
        "City tour - Visit landmarks and cultural sites.",
        "Explore local markets and shopping districts.",
        "Relaxation day at the beach or local park.",
        "Adventure day - Outdoor activities.",
        "Final day - Departure."
    };

        // Loop through the days and add the respective activity to the itinerary
        for (int day = 1; day <= days; day++)
        {
            // Check if the day exceeds the number of predefined activities
            if (day <= activities.Count)
            {
                Itinerary.Add($"Day {day}: {activities[day - 1]}");
            }
            else
            {
                Itinerary.Add($"Day {day}: Explore more activities.");
            }
        }
    }

    // Helper method to determine the price per room based on the selected room type
    private static decimal GetRoomPrice(string roomType)
    {
        switch (roomType)
        {
            case "Superior":
                return 1500; // INR 1500 for Superior Room
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


    private void CalculateTransportation(int days)
    {
        int totalPeople = Adults + Children;

        // Determine vehicle and rent based on total number of people
        if (totalPeople <= 4)
        {
            SuggestedVehicle = "Car";
            RentPerDay = 500; // Rent per day for Car
        }
        else if (totalPeople <= 5)
        {
            SuggestedVehicle = "SUV";
            RentPerDay = 800; // Rent per day for Mini Van
        }
        else if (totalPeople <= 6)
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
