
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
    public interface INewsArticleService
    {
        public Task CreateAsync(NewsArticleRequest articleDTO);
        public Task<bool> DeleteAsync(string id);
        Task<List<NewsArticleDTO>?> GetArticlesByStaffIdAsync(int staffId);
        Task<List<NewsArticleDTO>?> SearchAsync(string? keyword, int page, int size);
        public Task<List<NewsArticleDTO>> GetAllArticleAsync(int page, int size);
        public Task<NewsArticleDTO?> GetByIdAsync(string id);
        public Task<List<NewsArticleDTO>> ShowNewAsync(DateTime? startDate, DateTime? endDate, int page, int size);
        public Task UpdateAsync(NewsArticleDTO articleDTO);
    }
}
