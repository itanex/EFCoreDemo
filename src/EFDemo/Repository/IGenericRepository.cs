using System;
using System.Linq;

namespace EFDemo.Repository
{
    public interface IGenericRepository: IDisposable
    {
        T Create<T>(T entityToCreate) where T : class;
        IQueryable<T> Read<T>() where T : class;

        void Update<T>(T entityToUpdate) where T : class;
        void Delete<T>(T entityToDelete) where T : class;
    }
}
