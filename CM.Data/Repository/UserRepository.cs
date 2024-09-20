using CM.Data.Repository.IRepository;
using CM.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Repository
{
	public class UserRepository: Repository<User>, IUserRepository 
	{
		private AppDbContext _context;
		public UserRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
