using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class NewsArticleDTO
    {
        public string? NewsArticleId { get; set; }

        public string? NewsTitle { get; set; }

        public string? Headline { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }
        public string? Category { get; set; }
        public bool NewsStatus { get; set; } = true;

        public short CreatedById { get; set; }

        public string? CreatedBy { get; set; }

        public short UpdatedById { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();

    }
}
