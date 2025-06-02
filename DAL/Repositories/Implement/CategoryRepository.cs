using DAL.Core;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;

        public CategoryRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            var categories = _context.Categories.ToList();

            if (categories.Count == 0)
            {
                category.ParentCategoryId = null;
            }
            else
            {
                if (category.ParentCategoryId != null &&
                    !categories.Any(c => c.CategoryId == category.ParentCategoryId))
                {
                    throw new Exception($"❌ ParentCategoryId {category.ParentCategoryId} không tồn tại.");
                }
            }

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(short id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) { return false; }
            category!.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedList<Category>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Categories
                .Include(x => x.ParentCategory)
                .Where(x => x.IsActive == true)
                .AsNoTracking();

            return await PaginatedList<Category>.CreateAsync(query, pageNumber, pageSize);
        }


        public async Task<Category?> GetByIdAsync(short id)
        {
            return await _context.Categories.Include(x => x.ParentCategory)
                .FirstOrDefaultAsync(x => x.CategoryId == id && x.IsActive == true);
        }

        public async Task<List<Category>> SearchAsync(string keyword)
        {
            return await _context.Categories
                .Where(x => x.CategoryName.Contains(keyword) && x.IsActive.Equals(true)).AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
