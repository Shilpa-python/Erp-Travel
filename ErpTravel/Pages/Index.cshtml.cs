﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
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

    [BindProperty]
    public string? AgencyName { get; set; }

    [BindProperty]
    public string? AgencyLocation { get; set; }

    [BindProperty]
    public int? AgencyNumber { get; set; }

    [BindProperty]
    public int Adults { get; set; } // Added BindProperty here

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

    public decimal PricePerBed { get; set; } = 500; // Example price for an extra bed

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

    // Calculate vehicle and rent logic based on total people
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

    // Initialize a sample itinerary for Singapore based on days
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
                    activity += "Visit Chinatown, Chinatown Heritage Centre and Fort Canning Park.";
                    break;
                default:
                    activity += "Free Day - Explore as you wish!";
                    break;
            }

            ItineraryList.Add(activity);
        }
    }

    // Generate a sample auto itinerary based on days
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

    // Handle form calculation based on selected room type and extras
    public void OnPostCalculate()
    {
        // Define pricing logic based on the room type selected
        switch (RoomType)
        {
            case "Superior-3star":
                PricePerRoom = 1500m;
                break;
            case "Superior-4star":
                PricePerRoom = 2000m;
                break;
            case "Superior-5star":
                PricePerRoom = 2500m;
                break;
            case "Deluxe-3star":
                PricePerRoom = 2000m;
                break;
            case "Deluxe-4star":
                PricePerRoom = 2500m;
                break;
            case "Deluxe-5star":
                PricePerRoom = 3000m;
                break;
            case "DeluxePremiere-3star":
                PricePerRoom = 2500m;
                break;
            case "DeluxePremiere-4star":
                PricePerRoom = 3000m;
                break;
            case "DeluxePremiere-5star":
                PricePerRoom = 3500m;
                break;
            case "FamilySuite-3star":
                PricePerRoom = 3000m;
                break;
            case "FamilySuite-4star":
                PricePerRoom = 3500m;
                break;
            case "FamilySuite-5star":
                PricePerRoom = 4000m;
                break;
            default:
                PricePerRoom = 1000m; // Default price for unhandled cases
                break;
        }

        // Calculate the total amount based on the number of rooms, extra beds, etc.
        TotalAmount = (Rooms * PricePerRoom * Nights) + (ChildrenWithBed * PricePerBed) + (ExtraBeds * PricePerBed);
    }

    public void OnPost()
    {
        if (AgencyNumber == 0)
        {
            ModelState.AddModelError("AgencyNumber", "Agency Number cannot be zero.");
            return;
        }
        int totalPeople = Adults + Children;
        CalculateVehicleAndRent(totalPeople);

        InitializeSingaporeItinerary(Days);

        TempData["Message"] = $"Booking successful! Total amount: INR {TotalAmount}. Vehicle suggested: {SuggestedVehicle}, Vehicle Rent: INR {TotalRent}.";
    }

    public IActionResult OnPostSubmit()
    {
        // Perform all necessary calculations
        int totalPeople = Adults + Children;
        CalculateVehicleAndRent(totalPeople);
        OnPostCalculate();  // Calculate total amount for rooms and other expenses

        if (ItineraryType == "auto")
        {
            GeneratedItinerary = GenerateAutoItinerary(Days);
        }
        string agencyName = Request.Form["AgencyName"];
        string agencyLocation = Request.Form["AgencyLocation"];
        
        // Save the calculated data in TempData to pass to the Quotation page
        TempData["AgencyName"] = AgencyName;
        TempData["AgencyLocation"] = AgencyLocation;
        TempData["AgencyNumber"] = AgencyNumber;
        TempData["ArrivalDate "] = ArrivalDate;
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
        TempData["Itinerary"] = ItineraryList;  // Save itinerary to TempData for the Quotation page

        // Redirect to the Quotation page after form submission
        return RedirectToPage("Quotation");
    }
}
