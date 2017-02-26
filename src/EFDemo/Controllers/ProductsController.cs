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

        // GET api/products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var items = db.Products
                .Include(x => x.Category)
                .ToList();

            return items;
        }

        // GET api/products/1
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var item = db.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST api/products
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

        // PUT api/products/1
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]ProductWriteVm newProduct)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get existing product from DB
            var product = db.Products.FirstOrDefault(x => x.Id == id);

            // Verify that the product exist
            if (product == null)
            {
                return NotFound();
            }

            // Assign updated Properties
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Category = db.Categories.FirstOrDefault(x => x.Id == newProduct.CategoryId);

            // Save
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/products/1
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var item = db.Products
                // Because there are no tables connecting to Product.Id
                // We do not need to include anything to insure deletion
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            db.Remove(item);
            db.SaveChanges();

            return NoContent();
        }

    }
}
