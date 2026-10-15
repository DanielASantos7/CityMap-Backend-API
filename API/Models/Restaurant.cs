using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public decimal Rating { get; set; }

    public string? PriceRange { get; set; }

    public string? RestaurantType { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public decimal Distance { get; set; }

    public string BusinessType { get; set; } = null!;

    public string? BusinessHours { get; set; }

    public int? RemainingParkingSpaces { get; set; }
}
