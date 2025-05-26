﻿using MyApp.Domain.Common;

namespace MyApp.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
