using System.Collections.Generic;

namespace EFDemo.Models
{
    public class CategoryReadVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductReadVm> Products { get; set; }
    }
}
