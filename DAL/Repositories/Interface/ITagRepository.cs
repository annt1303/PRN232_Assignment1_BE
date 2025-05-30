using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interface
{
    public interface ITagRepository
    {
        public Task<List<Tag>?> GetAllAsync();
        public Task<List<Tag>> GetTagsById(List<int> tagIds);
        public void RemoveListTags(List<Tag> tags);
    }
}
