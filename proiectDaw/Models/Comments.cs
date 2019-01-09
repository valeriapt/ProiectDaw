using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
	public class Comments
	{
		[Key]
		public int Id { get; set; }
		public int PictureID { get; set; }
		[Required]
		public string Text { get; set; }
	
		public string userId { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }

		public Pictures Picture;
		public virtual ApplicationUser User { get; set; }

	}

}