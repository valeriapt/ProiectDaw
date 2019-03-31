using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
	public class Albums
	{
		[Key]
		public int Id { get; set; }
		[Required][MinLength(5)]
		public string Name { get; set; }
		public string UserId { get; set; }  
         
		public string CreatedBy { get; set; }  
		public virtual ICollection<Pictures> Pictures{ get; set; }
		public virtual ApplicationUser User { get; set; }
	}
}