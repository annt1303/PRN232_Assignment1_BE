
using BLL.Models;
using BLL.Models.Request;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        public CategoryService(ICategoryRepository categoryRepository, INewsArticleRepository newsArticleRepository)
        {
            _categoryRepository = categoryRepository;
            _newsArticleRepository = newsArticleRepository;
        }
        public async Task AddAsync(CategoryRequest categoryDto)
        {
            await _categoryRepository.AddAsync(new Category
            {
                CategoryName = categoryDto.CategoryName,
                CategoryDesciption = categoryDto.CategoryDesciption,
                ParentCategoryId = categoryDto.ParentCategoryId,
                IsActive = true
            });
        }

        public async Task<bool> DeleteAsync(short id)
        {
            List<NewsArticle>? newsArticles = await _newsArticleRepository.GetArticlesByCategory(id);
            if (newsArticles.Count() != 0)
            {
                throw new Exception("Category is in use");
            }
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category> categories = await _categoryRepository.GetAllAsync();

            foreach (Category x in categories)
            {
                categoryDTOs.Add(MapToDTO(x));

            }

            return categoryDTOs;
        }
        public async Task<CategoryDTO?> GetByIdAsync(short id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            return MapToDTO(category);
        }

        public async Task<List<CategoryDTO>> SearchAsync(string keyword)
        {
            if (keyword == null)
            {
                return await GetAllAsync();
            }
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            List<Category>? categories = await _categoryRepository.SearchAsync(keyword);
            if (categories == null)
            {
                return null;
            }


            foreach (Category item in categories)
            {
                categoryDTOs.Add(MapToDTO(item));
            }
            return categoryDTOs;
        }
        public async Task UpdateAsync(CategoryDTO categoryDto)
        {

            Category? category = await _categoryRepository.GetByIdAsync(categoryDto.CategoryId);
            if (category != null)
            {
                category.CategoryName = categoryDto.CategoryName;
                category.CategoryDesciption = categoryDto.CategoryDesciption;
                category.ParentCategoryId = categoryDto.ParentCategoryId;
                await _categoryRepository.UpdateAsync(category);
            }


        }
        public CategoryDTO MapToDTO(Category category)
        {
            return new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.CategoryName ?? "N/A"
            };

        }
    }
}
