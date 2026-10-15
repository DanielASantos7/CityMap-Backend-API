using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RouteSegment
{
    public int SegmentId { get; set; }

    public int StartPointId { get; set; }

    public int EndPointId { get; set; }

    public string TrafficCondition { get; set; } = null!;

    public string ColorHex { get; set; } = null!;

    public int SequenceOrder { get; set; }

    public virtual MapPoint EndPoint { get; set; } = null!;

    public virtual MapPoint StartPoint { get; set; } = null!;
}
