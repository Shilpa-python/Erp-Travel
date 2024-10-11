using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

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

    public List<string> Itinerary { get; set; } = new List<string>();

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
        Itinerary = new List<string>();

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

            Itinerary.Add(activity);
        }
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
            case "Del":
                PricePerRoom = 2000m;
                break;
            case "Del prem":
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

        // Convert Itinerary list to TempData format
        TempData["Itinerary"] = Itinerary;

        // Redirect to the Quotation page
        return RedirectToPage("/Quotation");
    }
}

