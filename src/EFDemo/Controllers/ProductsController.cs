using EFDemo.Models;
using EFDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EFDemo.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private ProductService service;

        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        // GET api/products
        [HttpGet]
        public IEnumerable<ProductReadVm> Get()
        {
            return service.GetAllProducts();
        }

        // GET api/products/1
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
