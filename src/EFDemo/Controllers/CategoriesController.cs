using EFDemo.Models;
using EFDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFDemo.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private CategoryService service;

        public CategoriesController(CategoryService service)
        {
            this.service = service;
        }

        // GET api/categories
        [HttpGet]
        public IEnumerable<CategoryReadVm> Get()
        {
            return service.GetAllCategories();
        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var item = service.GetCategoryById(id);

            if (item == null)
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

            var category = service.CreateCategory(newCategory);

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

            var category = service.UpdateCategory(id, newCategory);

            // Verify that the category was updated
            if (category == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/categories/1
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var success = service.DeleteCategory(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
