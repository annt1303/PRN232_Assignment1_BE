
using BLL.Models;
using BLL.ServiceInterface;
using DAL.Models;
using DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceImp
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly ITagRepository _tagRepository;
        public NewsArticleService(INewsArticleRepository newsArticleRepository, ISystemAccountRepository systemAccountRepository, ITagRepository tagRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _systemAccountRepository = systemAccountRepository;
            _tagRepository = tagRepository;
        }

        public async Task<List<NewsArticleDTO>> GetAllArticleAsync()
        {
            List<NewsArticleDTO> newDTOs = new List<NewsArticleDTO>();
            List<NewsArticle> news = await _newsArticleRepository.GetAllAsync();

            foreach (NewsArticle x in news)
            {
                newDTOs.Add(await MapToDTO(x));

            }

            return newDTOs;
        }

        public async Task<List<NewsArticleDTO>> ShowNewAsync(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate == null)
            {
                return await GetAllArticleAsync();
            }
            List<NewsArticleDTO> newDTOs = new List<NewsArticleDTO>();
            List<NewsArticle> news = await _newsArticleRepository.ShowNewAsync(startDate, endDate);
            if (news == null)
            {
                return null;
            }
            foreach (NewsArticle x in news)
            {
                newDTOs.Add(await MapToDTO(x));

            }

            return newDTOs;
        }
        public async Task<NewsArticleDTO> MapToDTO(NewsArticle newsArticle)
        {
            short userId = 0;
            if (newsArticle.UpdatedById.HasValue)
            {
                userId = newsArticle.UpdatedById.Value;
            }
            SystemAccount? user = await _systemAccountRepository.GetAccountById(userId);
            return new NewsArticleDTO
            {
                NewsArticleId = newsArticle.NewsArticleId,
                NewsTitle = newsArticle.NewsTitle,

                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                Headline = newsArticle.Headline,
                CreatedDate = newsArticle.CreatedDate,
                CreatedBy = newsArticle.CreatedBy?.AccountName ?? "Unknown",
                ModifiedDate = newsArticle.ModifiedDate,
                UpdatedBy = user?.AccountName ?? "N/A",
                CategoryId = newsArticle.CategoryId,
                Category = newsArticle.Category?.CategoryName ?? "Uncategorized",
                TagIds = MapToDTO(newsArticle.Tags)
            };
        }
        public List<int> MapToDTO(ICollection<Tag> newsTags)
        {
            List<int> tagDTOs = new List<int>();
            foreach (Tag x in newsTags)
            {
                tagDTOs.Add(x.TagId);
            }
            return tagDTOs;
        }

        public async Task CreateAsync(NewsArticleDTO articleDTO)
        {
            var newsArticle = new NewsArticle
            {
                NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 17),
                NewsTitle = articleDTO.NewsTitle,
                Headline = articleDTO.Headline,
                CreatedDate = DateTime.Now,
                NewsContent = articleDTO.NewsContent,
                NewsSource = articleDTO.NewsSource,
                ModifiedDate = DateTime.Now,
                NewsStatus = true,
                UpdatedById = articleDTO.CreatedById,
                CreatedById = articleDTO.CreatedById,
                CategoryId = articleDTO.CategoryId,

            };
            var tags = await _tagRepository.GetTagsById(articleDTO.TagIds);
            foreach (Tag x in tags)
            {
                newsArticle.Tags.Add(x);
            }
            await _newsArticleRepository.CreateAsync(newsArticle);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _newsArticleRepository.DeleteAsync(id);
        }

        public async Task<NewsArticleDTO?> GetByIdAsync(string id)
        {
            var newArticle = await _newsArticleRepository.GetByIdAsync(id);
            if (newArticle == null)
            {
                return null;
            }
            return await MapToDTO(newArticle);
        }

        public async Task UpdateAsync(NewsArticleDTO articleDTO)
        {
            var existingArticle = await _newsArticleRepository.GetByIdAsync(articleDTO.NewsArticleId!);

            if (existingArticle == null)
                return;

            existingArticle.NewsTitle = articleDTO.NewsTitle;
            existingArticle.Headline = articleDTO.Headline;
            existingArticle.NewsContent = articleDTO.NewsContent;
            existingArticle.NewsSource = articleDTO.NewsSource;
            existingArticle.UpdatedById = articleDTO.UpdatedById;
            existingArticle.ModifiedDate = DateTime.Now;
            existingArticle.CategoryId = articleDTO.CategoryId;
            await _newsArticleRepository.RemoveListTags(existingArticle);
            var tags = await _tagRepository.GetTagsById(articleDTO.TagIds);
            foreach (Tag x in tags)
            {
                existingArticle.Tags.Add(x);
            }
            await _newsArticleRepository.UpdateAsync(existingArticle);
        }

        public async Task<List<NewsArticleDTO>?> GetArticlesByStaffIdAsync(int staffId)
        {
            List<NewsArticleDTO> newDTOs = new List<NewsArticleDTO>();
            List<NewsArticle> news = await _newsArticleRepository.GetArticlesByStaffIdAsync(staffId);

            foreach (NewsArticle x in news)
            {
                newDTOs.Add(await MapToDTO(x));

            }

            return newDTOs;
        }

        public async Task<List<NewsArticleDTO>?> SearchAsync(string? keyword)
        {
            if (keyword == null)
            {
                return await GetAllArticleAsync();
            }
            List<NewsArticleDTO> newDTOs = new List<NewsArticleDTO>();
            List<NewsArticle> news = await _newsArticleRepository.SearchAsync(keyword);
            if (news == null)
            {
                return null;
            }
            foreach (NewsArticle x in news)
            {
                newDTOs.Add(await MapToDTO(x));

            }
            return newDTOs;
        }
    }
}
