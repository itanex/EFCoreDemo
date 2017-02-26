namespace EFDemo.Models
{
    public class ProductReadVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public CategoryReadVm Category { get; set; }
    }
}
