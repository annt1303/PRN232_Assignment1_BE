using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interface
{
    public interface INewsArticleRepository
    {
        Task<List<NewsArticle>> GetArticlesByStaffIdAsync(int staffId);
        Task<List<NewsArticle>> GetArticlesByCategory(short categoryId);
        Task<List<NewsArticle>> GetAllAsync();
        Task<List<NewsArticle>> SearchAsync(string keyword);
        Task<List<NewsArticle>> ShowNewAsync(DateTime? startDate, DateTime? endDate);
        Task<NewsArticle?> GetByIdAsync(string id);
        Task CreateAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle articleDTO);
        Task<bool> DeleteAsync(string id);
        Task RemoveListTags(NewsArticle newsArticle);
    }
}
