using EFDemo.Models;
using EFDemo.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDemo.Services
{
    public class ProductService
    {
        private IGenericRepository repo;

        public ProductService(IGenericRepository repo)
        {
            this.repo = repo;
        }

        public IEnumerable<ProductReadVm> GetAllProducts()
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

        public ProductReadVm GetProductById(int id)
        {
            var item = repo.Read<Product>()
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return null;
            }

            return new ProductReadVm()
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
            };
        }

        public ProductReadVm CreateProduct(ProductWriteVm newProduct)
        {
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
            var item = repo.Create(product);

            return new ProductReadVm()
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
            };
        }

        public ProductReadVm UpdateProduct(int id, ProductWriteVm newProduct)
        {
            // Get existing product from DB
            var product = repo.Read<Product>()
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            // Verify that the product exist
            if (product == null)
            {
                return null;
            }

            // Assign updated Properties
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Category = repo.Read<Category>().FirstOrDefault(x => x.Id == newProduct.CategoryId);

            repo.Update(product);

            return new ProductReadVm()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                InStock = product.InStock,
                Category = (product.Category == null) ? null : new CategoryReadVm()
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                }
            };
        }

        public bool DeleteProduct(int id)
        {
            var item = repo.Read<Product>()
                // Because there are no tables connecting to Product.Id
                // We do not need to include anything to insure deletion
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return false;
            }

            repo.Delete(item);

            return true;
        }
    }
}
