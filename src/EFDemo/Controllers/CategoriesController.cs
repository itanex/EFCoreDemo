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
        private IAntiforgery antiforgery;
        private ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db, IAntiforgery antiforgery)
        {
            this.db = db;
            this.antiforgery = antiforgery;
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

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<CategoryVm> Get()
        //{
        //    var items = db.Categories
        //        .Include(x => x.Products)
        //        .Select(x => new CategoryVm()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            Products = x.Products.Select(y => new ProductVm()
        //            {
        //                Id = y.Id,
        //                Name = y.Name,
        //                Price = y.Price,
        //                InStock = y.InStock
        //            })
        //        });

        //    return items;
        //}
    }
}
