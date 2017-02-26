using EFDemo.Models;
using EFDemo.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EFDemo.Services
{
    public class CategoryService
    {
        private IGenericRepository repo;

        public CategoryService(IGenericRepository repo)
        {
            this.repo = repo;
        }

        public IEnumerable<CategoryReadVm> GetAllCategories()
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

        public CategoryReadVm GetCategoryById(int id)
        {
            var item = repo.Read<Category>()
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return null;
            }

            return new CategoryReadVm()
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
            };
        }

        public CategoryReadVm CreateCategory(CategoryWriteVm newCategory)
        {
            // Create a new Category Domain Object
            var category = new Category()
            {
                Name = newCategory.Name
            };

            // Associate any provided products
            category.Products = repo.Read<Product>().Where(x => newCategory.ProductIds.Contains(x.Id)).ToArray();

            // Attach category to DB
            var item = repo.Create(category);

            return new CategoryReadVm()
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
            };
        }

        public CategoryReadVm UpdateCategory(int id, CategoryWriteVm newCategory)
        {
            // Get existing category from DB
            var category = repo.Read<Category>()
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == id);

            // Verify that the category exist
            if (category == null)
            {
                return null;
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

            return new CategoryReadVm()
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(p => new ProductReadVm()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    InStock = p.InStock
                })
            };
        }

        public bool DeleteCategory(int id)
        {
            var item = repo.Read<Category>()
                // Because of the relationship, if we do not include products 
                // which has a foreign keys we cannot remove the category
                .Include(x => x.Products)
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
