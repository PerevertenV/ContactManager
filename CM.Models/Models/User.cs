using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Models.Models
{
	public class User
	{
		[Key]
		public int ID { get; set; }
		[Required]
		[StringLength(20)]
		[DisplayName("Login")]
		public string Login { get; set; }
		[Required]
		[DisplayName("Password")]
		public string Password { get; set; }
	}
}
