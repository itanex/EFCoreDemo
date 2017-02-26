using EFDemo.Data;
using System.Linq;

namespace EFDemo.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private ApplicationDbContext db;

        public GenericRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public T Create<T>(T entityToCreate) where T : class
        {
            db.Set<T>().Add(entityToCreate);
            SaveChanges();

            return entityToCreate;
        }

        public IQueryable<T> Read<T>() where T : class
        {
            return db.Set<T>().AsQueryable();
        }

        public void Update<T>(T entityToUpdate) where T : class
        {
            db.Set<T>().Update(entityToUpdate);
            SaveChanges();
        }

        public void Delete<T>(T entityToDelete) where T : class
        {
            db.Set<T>().Remove(entityToDelete);
            SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
