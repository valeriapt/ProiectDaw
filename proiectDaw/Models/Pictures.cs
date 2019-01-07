using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
	public class Pictures
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		//[Required]
		//public byte[] Image { get; set; }
		public string ImagePath { get; set; }
		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
		public string UserId { get; set; }
		[Required]
		public int CategoryId { get; set; }
		public string Description { get; set; }
		public int AlbumId { get; set; }
		public DateTime Date { get; set; }

		public virtual ApplicationUser User { get; set; }
		public virtual ICollection<Comments> Comments { get; set; }

        //TODO: adaugare relatia "o poza apartine unei categorii"
	}
}