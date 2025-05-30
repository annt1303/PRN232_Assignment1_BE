
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterface
{
    public interface ITagService
    {
        public Task<List<TagDTO>> GetAllAsync();
    }
}
