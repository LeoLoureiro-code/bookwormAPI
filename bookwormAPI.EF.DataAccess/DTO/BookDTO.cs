using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookwormAPI.EF.DataAccess.DTO
{
    public class BookDTO
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public int Pages { get; set; }

        public string Status { get; set; } = null!;

        public string? Feeling { get; set; }
    }
}
