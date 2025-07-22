using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.DTO
{
    public class BookDTO
    {
        public string BookTitle { get; set; } = null!;

        public string BookAuthor { get; set; } = null!;

        public int BookPages { get; set; }

        public string BookStatus { get; set; } = null!;

        public string? BookFeeling { get; set; }
    }
}
