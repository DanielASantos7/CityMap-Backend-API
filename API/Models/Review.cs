using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public string BusinessType { get; set; } = null!;

    public int BusinessId { get; set; }

    public int Rating { get; set; }

    public string ReviewContent { get; set; } = null!;

    public DateTime ReviewTime { get; set; }

    public int LikeCount { get; set; }

    public virtual ICollection<ReviewImage> ReviewImages { get; set; } = new List<ReviewImage>();

    public virtual User User { get; set; } = null!;
}
