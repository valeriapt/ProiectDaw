using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Models
{
    public class Likes
    {
        [Key] 
        public int Id { get; set; }
       
        public string UserId { get; set; }
      
        public int PhotoId { get; set; }
        public int Rating { get; set; }

        public virtual Pictures Picture { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}