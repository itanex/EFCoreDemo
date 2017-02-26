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

        // GET api/values
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var items = db.Categories
                .Include(x => x.Products)
                .ToList();

            return items;
        }

        // GET api/values/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
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

        // POST api/values
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
    }
}
