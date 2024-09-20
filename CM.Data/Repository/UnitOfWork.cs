using CM.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private AppDbContext _context;
		public IUserRepository User { get; private set; }
		public IContactInfoRepository ContactInfo { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
			_context = context;
			User = new UserRepository(_context);
			ContactInfo = new ContactInfoRepository(_context);
        }
    }
}
