using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class ViewModel
    {
        public List<Product> Products { get; set; }
        public Order Orders { get; set; }
        public Product TheProduct { get; set; }

    }
}
