using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class QuotationModel : PageModel
{
    public string? RoomType { get; set; }
    public int Rooms { get; set; }
    public int Nights { get; set; }
    public int ExtraBeds { get; set; }
    public int ChildrenWithBed { get; set; }
    public int ChildrenWithoutBed { get; set; }
    public decimal TotalAmount { get; set; }
    public string? SuggestedVehicle { get; set; }
    public decimal RentPerDay { get; set; }
    public decimal TotalRent { get; set; }
    public List<string>? Itinerary { get; set;  }
    public decimal GrandTotal { get; set; }

    public QuotationModel()
    {
        Itinerary = new List<string>();
    }
    public void OnPost()
    {
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
        TotalAmount = (Rooms * roomPricePerNight * Nights) +
                          (ChildrenWithBed * pricePerChildWithBed * Nights) +
                          (ExtraBeds * pricePerExtraBed * Nights);

        // Vehicle rent is already passed in the form and set directly
        // TotalVehicleRent comes from the hidden form field in the front-end

        // Now, the TotalAmount is automatically calculated using the TotalAmount property (TotalRoomAmount + TotalVehicleRent)
    }

    // Helper method to determine the price per room based on the selected room type
    private static decimal GetRoomPrice(string roomType)
    {
        switch (roomType)
        {
            case "Superior":
                return 1500; // INR 1500 for Superior Room
            case "Del":
                return 2000; // INR 2000 for Deluxe Room
            case "Del prem":
                return 2500; // INR 2500 for Deluxe Premiere Room
            case "Fam suite":
                return 3000; // INR 3000 for Family Suite
            default:
                return 0; // Default case for invalid room type
        }
    }
}
