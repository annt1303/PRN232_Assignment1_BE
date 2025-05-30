using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CategoryDTO
    {
        public short CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryDesciption { get; set; }

        public short? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
