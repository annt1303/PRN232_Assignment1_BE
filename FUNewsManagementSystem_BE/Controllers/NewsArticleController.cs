
using BLL.Models;
using BLL.Models.Request;
using BLL.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ILogger<NewsArticlesController> _logger;

        public NewsArticlesController(INewsArticleService newsArticleService, ILogger<NewsArticlesController> logger)
        {
            _newsArticleService = newsArticleService;
            _logger = logger;
        }

        // GET: api/NewsArticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticleDTO>>> GetAll()
        {
            var result = await _newsArticleService.GetAllArticleAsync();
            return Ok(result);
        }

        // GET: api/NewsArticles/show?startDate=2025-05-01&endDate=2025-05-31
        [HttpGet("show")]
        public async Task<ActionResult<IEnumerable<NewsArticleDTO>>> Show([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _newsArticleService.ShowNewAsync(startDate, endDate);
            return Ok(result);
        }

        // GET: api/NewsArticles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsArticleDTO>> GetById(string id)
        {
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        // POST: api/NewsArticles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsArticleRequest article)
        {
            await _newsArticleService.CreateAsync(article);
            return Ok();
        }

        // PUT: api/NewsArticles
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NewsArticleDTO article)
        {
            await _newsArticleService.UpdateAsync(article);
            return NoContent();
        }

        // DELETE: api/NewsArticles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _newsArticleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // GET: api/NewsArticles/staff/{staffId}
        [HttpGet("staff/{staffId}")]
        public async Task<ActionResult<IEnumerable<NewsArticleDTO>>> GetByStaffId(int staffId)
        {
            var result = await _newsArticleService.GetArticlesByStaffIdAsync(staffId);
            return Ok(result);
        }

        // GET: api/NewsArticles/search?keyword=abc
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NewsArticleDTO>>> Search([FromQuery] string? keyword)
        {
            var result = await _newsArticleService.SearchAsync(keyword);
            return Ok(result);
        }
    }
}
