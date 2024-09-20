using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IUserRepository User { get; }
		IContactInfoRepository ContactInfo { get; }
	}
}
