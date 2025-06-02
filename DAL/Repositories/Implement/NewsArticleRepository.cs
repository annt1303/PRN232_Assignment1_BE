using DAL.Core;
using DAL.Models;
using DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repositories.Implement
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsArticleRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var articleToDelete = await _context.NewsArticles.FirstOrDefaultAsync(na => na.NewsArticleId == id);

            if (articleToDelete == null)
                return false;

            articleToDelete.NewsStatus = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedList<NewsArticle>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.NewsArticles
                .Include(x => x.CreatedBy)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Where(x => x.NewsStatus == true)
                .OrderByDescending(a => a.CreatedDate)
                .AsNoTracking();

            return await PaginatedList<NewsArticle>.CreateAsync(query, pageNumber, pageSize);
        }


        public async Task<List<NewsArticle>> GetArticlesByCategory(short categoryId)
        {
            return await _context.NewsArticles
                .Where(x => x.NewsStatus == true && x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetArticlesByStaffIdAsync(int staffId)
        {
            return await _context.NewsArticles
                .Include(x => x.CreatedBy)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Where(x => x.NewsStatus == true && x.CreatedById == staffId)
                .OrderByDescending(a => a.CreatedDate)
                .AsNoTracking().ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _context.NewsArticles.FirstOrDefaultAsync(na => na.NewsArticleId == id && na.NewsStatus == true);
        }

        public async Task RemoveListTags(NewsArticle newsArticle)
        {
            var article = await _context.NewsArticles
                        .Include(na => na.Tags)
                        .FirstOrDefaultAsync(na => na.NewsArticleId == newsArticle.NewsArticleId);

            if (article != null)
            {

                article.Tags.ToList().ForEach(tag => article.Tags.Remove(tag));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsArticle>> SearchAsync(string keyword)
        {
            return await _context.NewsArticles
                .Include(x => x.CreatedBy)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Where(a => (a.NewsTitle.Contains(keyword) &&
                            a.NewsStatus == true))
               .OrderByDescending(a => a.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> ShowNewAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _context.NewsArticles
                .Include(x => x.CreatedBy)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .Where(a => (!startDate.HasValue || a.CreatedDate.Value.Date >= startDate.Value.Date) &&
                            (!endDate.HasValue || a.CreatedDate.Value.Date <= endDate.Value.Date) &&
                            a.NewsStatus == true)
               .OrderByDescending(a => a.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(NewsArticle article)
        {

            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();

        }
    }
}
