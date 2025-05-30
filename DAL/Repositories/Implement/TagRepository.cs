using DAL.Models;
using DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implement
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementContext _context;

        public TagRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>?> GetAllAsync()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<List<Tag>> GetTagsById(List<int> tagIds)
        {
            return await _context.Tags
        .Where(t => tagIds.Contains(t.TagId))
        .ToListAsync();
        }

        public void RemoveListTags(List<Tag> tags)
        {
            _context.Tags.RemoveRange(tags);
        }
    }
}
