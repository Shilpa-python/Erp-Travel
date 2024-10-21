using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class QuotationModel : PageModel
{
    [BindProperty]
    public string? AgencyName { get; set; }

    [BindProperty]
    public string? AgencyLocation { get; set; }

    [BindProperty]
    public DateTime ArrivalDate { get; set; }

    [BindProperty]
    public int Adults { get; set; }

    [BindProperty]
    public int Children { get; set; }

    [BindProperty]
    public int Rooms { get; set; }
    [BindProperty]
    public int ChildrenWithBed { get; set; }

    [BindProperty]
    public int ChildrenWithoutBed { get; set; }

    [BindProperty]
    public int ExtraBeds { get; set; }


    [BindProperty]
    public int Nights { get; set; }

    [BindProperty]
    public int Days { get; set; }




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

    
   /* public class RoomPreference
    {
        public string? roomType { get; set; }
        public string? pricePerRoom { get; set; }
        public decimal totalPrice { get; set; }
    }*/





public decimal PerHeadAdultCost { get; set; }
    public decimal PerHeadChildWithBedCost { get; set; }
    public decimal PerHeadChildWithoutBedCost { get; set; }
    public decimal PerHeadExtraBedCost { get; set; }
    public List<string> Itinerary { get; set; }
    public string? SelectedItinerary { get; set; }
    public QuotationModel()
    {
        Itinerary = new List<string>();
    }

    public void OnGet()
    {
        // Retrieving values from TempData
        AgencyName = TempData["AgencyName"] as string;
        AgencyLocation = TempData["AgencyLocation"] as string;
        RoomType = TempData["RoomType"] as string; // Check this value
        Rooms = TempData["Rooms"] as int? ?? 0;
        Nights = TempData["Nights"] as int? ?? 0;
        ExtraBeds = TempData["ExtraBeds"] as int? ?? 0;
        ChildrenWithBed = TempData["ChildrenWithBed"] as int? ?? 0;
        ChildrenWithoutBed = TempData["ChildrenWithoutBed"] as int? ?? 0;

        // Handling decimal conversions for safer operations
        TotalRoomAmount = 0; // Set a default or computed value
        SuggestedVehicle = TempData["SuggestedVehicle"] as string;
        RentPerDay = TempData["RentPerDay"] as decimal? ?? 0;
        TotalRent = TempData["TotalRent"] as decimal? ?? 0;

        // Generating the itinerary if present in TempData
        SelectedItinerary = TempData["Itinerary"] as string;
        if (!string.IsNullOrEmpty(SelectedItinerary))
        {
            string[] itineraryItems = SelectedItinerary.Split('\n');
            Itinerary.AddRange(itineraryItems);
        }
    }

    public void OnPost()
    {
        // Ensure Days are correctly calculated from Nights
        Days = Nights + 1;

        // Calculate transportation based on the total number of people and days
        CalculateTransportation(Days);

        // Validate RoomType
        if (string.IsNullOrEmpty(RoomType))
        {
            ModelState.AddModelError("RoomType", "Please select a valid room type.");
            return;
        }

        // Calculate room costs and total amounts
        decimal roomPricePerNight = GetRoomPrice(RoomType);
        decimal pricePerChildWithBed = 150; // Assuming INR 150 per child with a bed
        decimal pricePerExtraBed = 150; // Assuming INR 150 per extra bed

        // Calculate room cost
        TotalRoomAmount = (Rooms * roomPricePerNight * Nights) +
                          (ChildrenWithBed * pricePerChildWithBed) +
                          (ExtraBeds * pricePerExtraBed);

        // Calculate per-head costs
        CalculatePerHeadCosts(roomPricePerNight, pricePerChildWithBed, pricePerExtraBed);
    }

    // Method to generate an itinerary based on days (if needed)
    private void GenerateItinerary(int days)
    {
        Itinerary.Clear();

        var activities = new List<string>
        {
            "Arrival and transfer to the hotel.",
            "City tour - Visit landmarks and cultural sites.",
            "Explore local markets and shopping districts.",
            "Relaxation day at the beach or local park.",
            "Adventure day - Outdoor activities.",
            "Final day - Departure."
        };

        // Loop through the days and add activities
        for (int day = 1; day <= days; day++)
        {
            if (day <= activities.Count)
            {
                Itinerary.Add($"Day {day}: {activities[day - 1]}");
            }
            else
            {
                Itinerary.Add($"Day {day}: Custom itinerary for this day.");
            }
        }
    }

    // Helper method to get the room price based on the selected room type
    private static decimal GetRoomPrice(string roomType)
    {
        return roomType switch
        {
            // 3-Star Hotel Room Prices
            "Superior-3star" => 1500m,
            "Deluxe-3star" => 2000m,
            "DeluxePremiere-3star" => 3000m,
            "FamilySuite-3star" => 4000m,

            // 4-Star Hotel Room Prices
            "Superior-4star" => 2000m,
            "Deluxe-4star" => 3000m,
            "DeluxePremiere-4star" => 4000m,
            "FamilySuite-4star" => 5000m,

            // 5-Star Hotel Room Prices
            "Superior-5star" => 3000m,
            "Deluxe-5star" => 4000m,
            "DeluxePremiere-5star" => 5000m,
            "FamilySuite-5star" => 6000m,

            // Default case for invalid room types
            _ => 1000m
        };
    }

    // Method to calculate transportation based on total people and days
    private void CalculateTransportation(int days)
    {
        int totalPeople = Adults + Children;

        // Determine vehicle and rent based on total number of people
        if (totalPeople <= 4)
        {
            SuggestedVehicle = "Car";
            RentPerDay = 500;
        }
        else if (totalPeople <= 5)
        {
            SuggestedVehicle = "SUV";
            RentPerDay = 800;
        }
        else if (totalPeople <= 7)
        {
            SuggestedVehicle = "Mini Van";
            RentPerDay = 1000;
        }
        else
        {
            SuggestedVehicle = "Bus";
            RentPerDay = 1500;
        }

        // Calculate total rent
        TotalRent = RentPerDay * days;
    }

    // Method to calculate per-head costs for adults, children with/without beds, and extra beds
    private void CalculatePerHeadCosts(decimal roomPricePerNight, decimal pricePerChildWithBed, decimal pricePerExtraBed)
    {
        // Total room cost for adults, children with bed, and extra beds
        decimal totalRoomCostForAdults = (Rooms * roomPricePerNight * Nights);
        decimal totalChildWithBedCost = (ChildrenWithBed * pricePerChildWithBed);
        decimal totalExtraBedCost = (ExtraBeds * pricePerExtraBed);

        // Calculate per-head adult cost
        if (Adults > 0)
        {
            PerHeadAdultCost = totalRoomCostForAdults / Adults;
        }
        else
        {
            PerHeadAdultCost = 0;
        }

        // Calculate per-head child with bed cost
        if (ChildrenWithBed > 0)
        {
            PerHeadChildWithBedCost = totalChildWithBedCost / ChildrenWithBed;
        }
        else
        {
            PerHeadChildWithBedCost = 0;
        }

        // Children without beds don't add to room cost, but we'll add a placeholder in case future calculations change
        PerHeadChildWithoutBedCost = 0;

        // Calculate per-head extra bed cost
        if (ExtraBeds > 0)
        {
            PerHeadExtraBedCost = totalExtraBedCost / ExtraBeds;
        }
        else
        {
            PerHeadExtraBedCost = 0;
        }

        // Correct handling for the total cost
        TotalRoomAmount = totalRoomCostForAdults + totalChildWithBedCost + totalExtraBedCost;
    }
}

