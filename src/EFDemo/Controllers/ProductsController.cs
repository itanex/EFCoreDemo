using EFDemo.Data;
using EFDemo.Models;
using EFDemo.Repository;
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
        private IGenericRepository repo;

        public ProductsController(IGenericRepository repo)
        {
            this.repo = repo;
        }

        // GET api/products
        [HttpGet]
        public IEnumerable<ProductReadVm> Get()
        {
            var items = repo.Read<Product>()
                .Include(x => x.Category)
                .Select(p => new ProductReadVm()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    InStock = p.InStock,
                    Category = (p.Category == null) ? null : new CategoryReadVm()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                });

            return items;
        }

        // GET api/products/1
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var item = repo.Read<Product>()
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(new ProductReadVm()
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                InStock = item.InStock,
                Category = (item.Category == null) ? null : new CategoryReadVm()
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name
                }
            });
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
            if (repo.Read<Product>().Any(x => x.Name.Equals(newProduct.Name, StringComparison.OrdinalIgnoreCase)))
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

            // Associate any provided products
            product.Category = repo.Read<Category>().FirstOrDefault(x => x.Id == newProduct.CategoryId);

            // Attach product to DB
            repo.Create(product);

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
            var product = repo.Read<Product>()
                .Include(x=> x.Category)
                .FirstOrDefault(x => x.Id == id);

            // Verify that the product exist
            if (product == null)
            {
                return NotFound();
            }

            // Assign updated Properties
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Category = repo.Read<Category>().FirstOrDefault(x => x.Id == newProduct.CategoryId);

            repo.Update(product);

            return NoContent();
        }

        // DELETE api/products/1
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var item = repo.Read<Product>()
                // Because there are no tables connecting to Product.Id
                // We do not need to include anything to insure deletion
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            repo.Delete(item);

            return NoContent();
        }
    }
}
