using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        public double Scorul { get; set; }
        public string User { get; set; }

    }
}