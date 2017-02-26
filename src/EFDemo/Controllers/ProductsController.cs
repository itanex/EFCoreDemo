using EFDemo.Data;
using EFDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFDemo.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db;

        public ProductsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var items = db.Products
                .Include(x => x.Category)
                .ToList();

            return items;
        }

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<ProductVm> Get()
        //{
        //    var items = db.Products
        //        .Include(x => x.Category)
        //        .Select(x => new ProductVm()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Price = x.Price,
        //            InStock = x.InStock,
        //            Category = new CategoryVm()
        //            {
        //                Id = x.Category.Id,
        //                Name = x.Category.Name
        //            }
        //        });

        //    return items;
        //}
    }
}
