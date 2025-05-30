using BLL.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // cả Admin và Staff đều có thể lấy tag
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _tagService.GetAllAsync());
    }

}
