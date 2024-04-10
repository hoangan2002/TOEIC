using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChillToeic.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
             _context.SaveChanges();
        }
		public void AddNoSaveChanges(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
			
		}
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
		public void Delete(int id)
        {
            var entity = GetById(id);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList(); 

        }
		public IEnumerable<TEntity>? Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
			if (includeProperties != null)
				foreach (var includeProperty in includeProperties)
					items = items.Include(includeProperty);

			if (predicate is not null)
				items = items.Where(predicate);

			return items;
		}


		public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
             _context.Set<TEntity>().Update(entity);
             _context.SaveChanges();
        }
    }
}
