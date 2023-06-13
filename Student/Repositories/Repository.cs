using Microsoft.EntityFrameworkCore;
using Student.Models;

namespace Student.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly stdDBContext _context;

        public Repository(stdDBContext context)
        {
            _context = context;
        }

        public void DeleteAll()
        {
            _context.Database.ExecuteSqlRaw($"DeleteALL");

        }
         public void DeleteAll(string table)
        {
            _context.Database.ExecuteSqlRaw($"Truncate table {table}");

        }

        public void DeleteById(int id)
        {

            T t = _context.Set<T>().Find(id);
            if (t != null)
            {
                _context.Set<T>().Remove(t);
                _context.SaveChanges();
            }

        }

        public void Editing(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }


        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
     

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
    }
}

