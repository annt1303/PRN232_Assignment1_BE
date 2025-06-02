
using BLL.Models;
using BLL.Models.Request;
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
        Task AddAsync(CategoryRequest categoryDto);
        Task<List<CategoryDTO>> GetAllAsync(int page, int size);
        Task<CategoryDTO?> GetByIdAsync(short id);

        Task UpdateAsync(CategoryDTO categoryDto);

        Task<bool> DeleteAsync(short id);

        Task<List<CategoryDTO>> SearchAsync(string keyword, int page, int size);

    }
}
