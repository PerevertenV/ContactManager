using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Models.Models
{
	public class ContactInfo
	{
		[Key]
        public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public DateOnly DateOfBirth { get; set; }
		[Required]
		public bool Married { get; set; }
		[Required]
		public string Phone { get; set; }
        [Required]
        public double Salary { get; set; }
    }
}
