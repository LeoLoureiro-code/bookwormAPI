using System;
using System.Collections.Generic;

namespace bookwormAPI.EF.DataAccess.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPasswordHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime RevokedAt { get; set; }

    public string? RefreshToken { get; set; }


    public bool IsActive { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();


}
