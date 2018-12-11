using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
	public class Categories
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public virtual ICollection<Pictures> Pictures { get; set; }
	}
}