using DAL.Core;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<PaginatedList<Category>> GetAllAsync(int pageNumber, int pageSize);

        Task<Category?> GetByIdAsync(short id);

        Task UpdateAsync(Category category);

        Task<bool> DeleteAsync(short id);

        Task<List<Category>> SearchAsync(string keyword);
    }
}
