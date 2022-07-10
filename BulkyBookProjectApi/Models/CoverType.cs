using System;
using System.Collections.Generic;

namespace BulkyBookProjectApi.Models
{
    public partial class CoverType
    {
        public CoverType()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
