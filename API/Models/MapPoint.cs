using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MapPoint
{
    public int PointId { get; set; }

    public string Name { get; set; } = null!;

    public string PointType { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public virtual ICollection<RouteSegment> RouteSegmentEndPoints { get; set; } = new List<RouteSegment>();

    public virtual ICollection<RouteSegment> RouteSegmentStartPoints { get; set; } = new List<RouteSegment>();
}
