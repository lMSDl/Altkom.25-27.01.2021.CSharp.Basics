﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Address : Entity
    {
        public ICollection<Person> Residents { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}