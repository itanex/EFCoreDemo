using EFDemo.Models;
using EFDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFDemo.Controllers
{
    /// <summary>
    /// Category Endpoint API
    /// </summary>
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private CategoryService service;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="service">DI Injected Category Service</param>
        public CategoriesController(CategoryService service)
        {
            this.service = service;
        }

        // GET api/categories
        /// <summary>
        /// Returns a list of all categories from the repository
        /// </summary>
        /// <response code="200">The repository returned categories</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryReadVm>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public IEnumerable<CategoryReadVm> Get()
        {
            return service.GetAllCategories();
        }

        // GET api/categories/1
        /// <summary>
        /// Find and return a specific category from the repository
        /// </summary>
        /// <param name="id">The id of the category to return</param>
        /// <response code="200">The requested category was found</response>
        /// <response code="404">No category with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryReadVm), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
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
        /// <summary>
        /// Creates the provided category in the repository
        /// </summary>
        /// <param name="newCategory">The new category to create</param>
        /// <response code="201">A new category was created in the repository</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryReadVm), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
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
        /// <summary>
        /// Creates the specific category in the repository 
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="newCategory">The full data of the category to update</param>
        /// <response code="204">The specified category in the repository was updated</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="404">No category with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
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
        /// <summary>
        /// Deletes the specific category from the repository
        /// </summary>
        /// <param name="id">The id of the category to delete</param>
        /// <response code="204">The expected category was removed from the repository</response>
        /// <response code="404">No category with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
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
