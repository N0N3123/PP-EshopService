using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Models
{
    public class CategoryModel
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string? Description { get; set; } 

       
        public ICollection<ProductModel>? Products { get; set; }
    }
}
