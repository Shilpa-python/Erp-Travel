using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Reflection;

public class IndexModel : PageModel
{
   
    public IActionResult OnGet()
    {
        
        // Check if the user is logged in
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
        {
            // If not logged in, redirect to the login page
            return RedirectToPage("/Login");
        }
        


        return Page();
    }

    [BindProperty]
    public string? AgencyName { get; set; }

    [BindProperty]
    public string? AgencyLocation { get; set; }

    [BindProperty]
    public string? AgencyNumber { get; set; }
    public int Adults { get; set; }

    [BindProperty]
    public int Children { get; set; }

    [BindProperty]
    public int Nights { get; set; }

    [BindProperty]
    public int Days { get; set; }

    [BindProperty]
    public DateTime ArrivalDate { get; set; }

    [BindProperty]
    public int Rooms { get; set; }

    [BindProperty]
    public string? RoomType { get; set; } 

    [BindProperty]
    public int ChildrenWithBed { get; set; }

    [BindProperty]
    public int ChildrenWithoutBed { get; set; }

    [BindProperty]
    public int ExtraBeds { get; set; }

    [BindProperty]
    public decimal PricePerRoom { get; set; }

    public decimal PricePerBed { get; set; }

    public decimal TotalAmount { get; set; }

    public string? SuggestedVehicle { get; set; }
    public decimal RentPerDay { get; set; }
    public decimal TotalRent { get; set; }

    [BindProperty]
    public string? ItineraryType { get; set; }

    [BindProperty]
    public string? Itinerary { get; set; }

    [BindProperty]
    public string? GeneratedItinerary { get; set; }
     public List<string> ItineraryList { get; set; } = new List<string>();
   
    

    private void CalculateVehicleAndRent(int totalPeople)
    {
        if (totalPeople <= 4)
        {
            SuggestedVehicle = "Car";
            RentPerDay = 500;
        }
        else if (totalPeople == 5)
        {
            SuggestedVehicle = "SUV";
            RentPerDay = 800;
        }
        else if (totalPeople == 6 || totalPeople == 7)
        {
            SuggestedVehicle = "Mini Van";
            RentPerDay = 1000;
        }
        else
        {
            SuggestedVehicle = "Bus";
            RentPerDay = 1500;
        }

        TotalRent = RentPerDay * Days;
     }

    private void InitializeSingaporeItinerary(int days)
    {
        ItineraryList = new List<string>();

        for (int i = 1; i <= days; i++)
        {
            string activity = $"Day {i}: ";

            switch (i)
            {
                case 1:
                    activity += "Arrival in Singapore - Transfer to hotel and leisure time.";
                    break;
                case 2:
                    activity += "Singapore City Tour - Visit Marina Bay Sands, Merlion Park, Chinatown.";
                    break;
                case 3:
                    activity += "Sentosa Island Tour - Explore Universal Studios, S.E.A. Aquarium, and more.";
                    break;
                case 4:
                    activity += "Free Day for Shopping and Local Exploration.";
                    break;
                case 5:
                    activity += "Visit Chinatown,Chinatown Heritage Centre and Fort Canning Park.";
                    break;
                default:
                    activity += "Free Day - Explore as you wish!";
                    break;
            }

            ItineraryList.Add(activity);
        }
    }

    private string GenerateAutoItinerary(int days)
    {
        string[] itineraryTemplate = new string[]
        {
                "Day 1: Arrival and transfer to the hotel.",
                "Day 2: City tour - Visit landmarks and cultural sites.",
                "Day 3: Explore local markets and shopping districts.",
                "Day 4: Relaxation day at the beach or local park.",
                "Day 5: Adventure day - Outdoor activities.",
                "Day 6: Final day - Departure."
        };

        string generatedItinerary = "";
        for (int i = 0; i < days; i++)
        {
            if (i < itineraryTemplate.Length)
            {
                generatedItinerary += itineraryTemplate[i] + "\n";
            }
            else
            {
                generatedItinerary += $"Day {i + 1}: Custom itinerary for this day.\n";
            }
        }

        return generatedItinerary;
    }

    public void OnPostCalculate()
    {
        // Calculate total amount based on rooms and selected room type
        // You can define the price per room according to room types
        switch (RoomType)
        {
            case "Superior":
                PricePerRoom = 1500m;
                break;
            case "Deluxe":
                PricePerRoom = 2000m;
                break;
            case "Deluxe prem":
                PricePerRoom = 2500m;
                break;
            case "Fam suite":
                PricePerRoom = 3000m;
                break;
            default:
                PricePerRoom = 1000m; // Default price
                break;
        }

        // Calculate total amount based on the number of rooms
        TotalAmount = (Rooms * PricePerRoom * Nights) + (ChildrenWithBed * PricePerBed) + (ExtraBeds * PricePerBed) ;
    }

    public void OnPost()
    {

        int totalPeople = Adults + Children;
        CalculateVehicleAndRent(totalPeople);

        InitializeSingaporeItinerary(Days);

        TempData["Message"] = $"Booking successful! Total amount:  INR {TotalAmount}. Vehicle suggested: {SuggestedVehicle}, Vehicle Rent: INR {TotalRent}.";

   
    }

    public IActionResult OnPostSubmit()
    {
        // Perform all necessary calculations here
        int totalPeople = Adults + Children;
        CalculateVehicleAndRent(totalPeople);
        OnPostCalculate();  // Calculate the total amount for rooms and other expenses
        if (ItineraryType == "auto")
        {
            GeneratedItinerary = GenerateAutoItinerary(Days);
        }
        // Save the data in TempData to pass to the Quotation page
        TempData["RoomType"] = RoomType;
        TempData["Rooms"] = Rooms;
        TempData["Nights"] = Nights;
        TempData["ExtraBeds"] = ExtraBeds;
        TempData["ChildrenWithBed"] = ChildrenWithBed;
        TempData["ChildrenWithoutBed"] = ChildrenWithoutBed;
        TempData["TotalAmount"] = TotalAmount;
        TempData["SuggestedVehicle"] = SuggestedVehicle;
        TempData["RentPerDay"] = RentPerDay;
        TempData["TotalRent"] = TotalRent;

        

        // Redirect to the Quotation page after form submission
        return RedirectToPage("Quotation");
    }
}

