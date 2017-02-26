using EFDemo.Data;
using EFDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ProductWriteVm newProduct)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify that the product does not already exist
            if (db.Products.Any(x => x.Name.Equals(newProduct.Name, StringComparison.OrdinalIgnoreCase)))
            {
                // Should be 409 - Conflict not 400 - Bad Request
                return BadRequest("Object Already Exists");
            }

            // Create a new Product Domain Object
            var product = new Product()
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                InStock = newProduct.InStock                
            };

            // Attach product to DB
            db.Products.Add(product);

            // Associate any provided products
            product.Category = db.Categories.FirstOrDefault(x => x.Id == newProduct.CategoryId);

            // Save
            db.SaveChanges();

            return Created("api/products" + product.Id, product);
        }
    }
}
