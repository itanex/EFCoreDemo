using EFDemo.Models;
using EFDemo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFDemo.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private IGenericRepository repo;

        public CategoriesController(IGenericRepository repo)
        {
            this.repo = repo;
        }

        // GET api/categories
        [HttpGet]
        public IEnumerable<CategoryReadVm> Get()
        {
            var items = repo.Read<Category>()
                .Include(x => x.Products)
                .Select(c => new CategoryReadVm()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new ProductReadVm()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        InStock = p.InStock
                    })
                });

            return items;
        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var item = repo.Read<Category>()
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(new CategoryReadVm()
            {
                Id = item.Id,
                Name = item.Name,
                Products = item.Products.Select(p => new ProductReadVm()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    InStock = p.InStock
                })
            });
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post([FromBody]CategoryWriteVm newCategory)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify that the category does not already exist
            if (repo.Read<Category>().Any(x => x.Name == newCategory.Name))
            {
                // Should be 409 - Conflict not 400 - Bad Request
                return BadRequest("Object Already Exists");
            }

            // Create a new Category Domain Object
            var category = new Category()
            {
                Name = newCategory.Name
            };

            // Associate any provided products
            category.Products = repo.Read<Product>().Where(x => newCategory.ProductIds.Contains(x.Id)).ToArray();

            // Attach category to DB
            repo.Create(category);

            return Created("api/categories" + category.Id, category);
        }

        // PUT api/categories/1
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]CategoryWriteVm newCategory)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get existing category from DB
            var category = repo.Read<Category>()
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            // Verify that the category exist
            if (category == null)
            {
                return NotFound();
            }

            // Assign updated Properties
            category.Name = newCategory.Name;

            foreach (var product in repo.Read<Product>().Where(x => newCategory.ProductIds.Contains(x.Id)))
            {
                category.Products.Add(product);
            }

            foreach (var product in repo.Read<Product>().Where(x => newCategory.RemoveProductIds.Contains(x.Id)))
            {
                category.Products.Remove(product);
            }

            repo.Update(category);

            return NoContent();
        }

        // DELETE api/categories/1
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var item = repo.Read<Category>()
                // Because of the relationship, if we do not include products 
                // which has a foreign keys we cannot remove the category
                .Include(x => x.Products)
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
