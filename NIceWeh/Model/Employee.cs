using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIceWeh.Model
{
    public class Employee
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string QueryTerm { get; set; }
        public override string ToString()
        {
            return Nome+" " +Cognome;
        }
    }
}