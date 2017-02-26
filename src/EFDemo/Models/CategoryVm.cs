using System.Collections.Generic;

namespace EFDemo.Models
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductVm> Products { get; set; }
    }
}
