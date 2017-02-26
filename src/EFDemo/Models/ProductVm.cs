namespace EFDemo.Models
{
    public class ProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public CategoryVm Category { get; set; }
    }
}
