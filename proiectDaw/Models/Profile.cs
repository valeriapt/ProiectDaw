using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
	public class Profile
	{
		[Key]
		public int Id { get; set; }
		public string  Username{ get; set; }
		public string UserId { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public int Age { get; set; }

		public string ProfileImagePath { get; set; }
		[NotMapped]
		public HttpPostedFileBase ProfileImageFile { get; set; }

		public virtual ICollection<Albums> Albums { get; set; }
		public virtual ApplicationUser User { get; set; }
	}
}