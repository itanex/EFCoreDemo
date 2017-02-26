using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFDemo.Models
{
    public class CategoryWriteVm
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<int> ProductIds { get; set; }
    }
}
