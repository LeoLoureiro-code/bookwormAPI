using System;
using System.Collections.Generic;

namespace bookwormAPI.EF.DataAccess.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string BookAuthor { get; set; } = null!;

    public int BookPages { get; set; }

    public string BookStatus { get; set; } = null!;

    public string BookFeeling { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
