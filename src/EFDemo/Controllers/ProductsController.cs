using EFDemo.Models;
using EFDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFDemo.Controllers
{
    /// <summary>
    /// Product Endpoint API
    /// </summary>
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private ProductService service;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="service">DI Injected Product Service</param>
        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        // GET api/products
        /// <summary>
        /// Returns a list of all products from the repository
        /// </summary>
        /// <response code="200">The repository returned products</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductReadVm>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public IEnumerable<ProductReadVm> Get()
        {
            return service.GetAllProducts();
        }

        // GET api/products/1
        /// <summary>
        /// Find and return a specific product from the repository
        /// </summary>
        /// <param name="id">The id of the product to return</param>
        /// <response code="200">The requested product was found</response>
        /// <response code="404">No product with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductReadVm), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public IActionResult Get([FromRoute]int id)
        {
            var item = service.GetProductById(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST api/products
        /// <summary>
        /// Creates the provided product in the repository
        /// </summary>
        /// <param name="newProduct">The new product to create</param>
        /// <response code="201">A new product was created in the repository</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductReadVm), 201)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public IActionResult Post([FromBody]ProductWriteVm newProduct)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = service.CreateProduct(newProduct);

            return Created("api/products" + product.Id, product);
        }

        // PUT api/products/1
        /// <summary>
        /// Creates the specific product in the repository 
        /// </summary>
        /// <param name="id">The id of the product to update</param>
        /// <param name="newProduct">The full data of the product to update</param>
        /// <response code="204">The specified product in the repository was updated</response>
        /// <response code="400">If the model is not valid</response>
        /// <response code="404">No product with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public IActionResult Put([FromRoute]int id, [FromBody]ProductWriteVm newProduct)
        {
            // Exercise ModelState Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Update the product
            var product = service.UpdateProduct(id, newProduct);

            // Verify that the product was updated
            if (product == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/products/1
        /// <summary>
        /// Deletes the specific product from the repository
        /// </summary>
        /// <param name="id">The id of the product to delete</param>
        /// <response code="204">The expected product was removed from the repository</response>
        /// <response code="404">No product with specified <paramref name="id"/> was found</response>
        /// <response code="500">If an exception is encountered</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public IActionResult Delete([FromRoute]int id)
        {
            var success = service.DeleteProduct(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
