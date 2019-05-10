using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiectDaw.Models
{
    public class Sugestii
    {
        [Key]
        public int Id { get; set; }
        public double Score { get; set; }
        public int PozaId { get; set; }
    }
}