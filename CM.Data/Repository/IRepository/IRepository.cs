using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Repository.IRepository
{
	public interface IRepository<T>
	{
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
		T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);

	}
}
