using System.Collections.Generic;

namespace EFDemo.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Property
        public virtual ICollection<Product> Products { get; set; }
    }
}
