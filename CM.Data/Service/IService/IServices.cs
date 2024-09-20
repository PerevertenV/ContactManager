using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Data.Service.IService
{
	public interface IServices
	{
		public IUserService User { get; }
		public IContactInfoService ContactInfo { get; }
	}
}
