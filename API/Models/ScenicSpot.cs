using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ScenicSpot
{
    public int SpotId { get; set; }

    public string SpotName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Phone { get; set; }

    public decimal Rating { get; set; }

    public string? TicketPrice { get; set; }

    public string? OpeningHours { get; set; }

    public string? TouristPeriod { get; set; }

    public string? Description { get; set; }

    public decimal Distance { get; set; }

    public string? ImageUrl { get; set; }

    public string BusinessType { get; set; } = null!;
}
