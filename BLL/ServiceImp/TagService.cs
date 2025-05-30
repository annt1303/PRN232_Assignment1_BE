
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
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task<List<TagDTO>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            if (tags == null)
            {
                return null;
            }
            return tags.Select(tag => MapToDTO(tag)).ToList();

        }
        private TagDTO MapToDTO(Tag tag)
        {
            return new TagDTO
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note
            };
        }

    }
}
