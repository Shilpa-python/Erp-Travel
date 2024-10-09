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
    public List<string> Itinerary { get; set;  }
public decimal GrandTotal { get; set; }

    public void OnGet()
    {
        // Load the data from TempData
        RoomType = TempData["RoomType"]?.ToString();
        Rooms = int.Parse(TempData["Rooms"]?.ToString() ?? "0");
        Nights = int.Parse(TempData["Nights"]?.ToString() ?? "0");
        ExtraBeds = int.Parse(TempData["ExtraBeds"]?.ToString() ?? "0");
        ChildrenWithBed = int.Parse(TempData["ChildrenWithBed"]?.ToString() ?? "0");
        ChildrenWithoutBed = int.Parse(TempData["ChildrenWithoutBed"]?.ToString() ?? "0");
        TotalAmount = decimal.Parse(TempData["TotalAmount"]?.ToString() ?? "0");
        SuggestedVehicle = TempData["SuggestedVehicle"]?.ToString();
        RentPerDay = decimal.Parse(TempData["RentPerDay"]?.ToString() ?? "0");
        TotalRent = decimal.Parse(TempData["TotalRent"]?.ToString() ?? "0");
        Itinerary = TempData["Itinerary"] as List<string> ?? new List<string>();
        GrandTotal = TotalAmount + TotalRent;
    }
}
