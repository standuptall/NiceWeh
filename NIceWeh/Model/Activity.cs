using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NIceWeh.Model
{
    public class Activity
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public override string ToString()
        {
            return Descrizione;
        }
    }
}