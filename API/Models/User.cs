using System;
using System.Collections.Generic;

namespace API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
