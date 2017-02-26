using EFDemo.Data;
using EFDemo.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDemo.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET api/categories
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var items = db.Categories
                .Include(x => x.Products)
                .ToList();

            return items;
        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var item = db.Categories
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
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
            if (db.Categories.Any(x => x.Name == newCategory.Name))
            {
                // Should be 409 - Conflict not 400 - Bad Request
                return BadRequest("Object Already Exists");
            }

            // Create a new Category Domain Object
            var category = new Category()
            {
                Name = newCategory.Name
            };

            // Attach category to DB
            db.Categories.Add(category);

            // Associate any provided products
            category.Products = db.Products.Where(x => newCategory.ProductIds.Contains(x.Id)).ToArray();

            // Save
            db.SaveChanges();

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
            var category = db.Categories
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            // Verify that the category exist
            if (category == null)
            {
                return NotFound();
            }

            // Assign updated Properties
            category.Name = newCategory.Name;

            foreach (var product in db.Products.Where(x => newCategory.ProductIds.Contains(x.Id)))
            {
                category.Products.Add(product);
            }

            foreach (var product in db.Products.Where(x => newCategory.RemoveProductIds.Contains(x.Id)))
            {
                category.Products.Remove(product);
            }

            // Save
            db.SaveChanges();

            return NoContent();
        }

        // DELETE api/categories/1
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var item = db.Categories
                // Because of the relationship, if we do not include products 
                // which has a foreign keys we cannot remove the category
                .Include(x => x.Products)
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
