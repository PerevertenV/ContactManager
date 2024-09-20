using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CM.Data.Service.IService;

namespace CM.Data.Service
{
	public class Services: IServices
	{
		public IUserService User { get; private set; }
		public IContactInfoService ContactInfo { get; private set; }

		public Services()
		{
			User = new UserService();
			ContactInfo = new ContactInfoService();
		}

	}
}
