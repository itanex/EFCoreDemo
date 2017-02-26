namespace EFDemo.Models
{
    public class ProductWriteVm
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
    }
}
