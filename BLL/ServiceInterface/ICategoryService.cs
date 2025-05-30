
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterface
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryDTO categoryDto);
        Task<List<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(short id);

        Task UpdateAsync(CategoryDTO categoryDto);

        Task<bool> DeleteAsync(short id);

        Task<List<CategoryDTO>> SearchAsync(string keyword);

    }
}
