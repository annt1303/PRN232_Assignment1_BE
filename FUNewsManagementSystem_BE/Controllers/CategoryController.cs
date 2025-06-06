﻿
using BLL.Models;
using BLL.Models.Request;
using BLL.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Staff")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
     => Ok(await _categoryService.GetAllAsync(page, size));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await _categoryService.SearchAsync(keyword, page, size));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequest dto)
        {
            await _categoryService.AddAsync(dto);
            return Ok("Category created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryDTO dto)
        {
            await _categoryService.UpdateAsync(dto);
            return Ok("Category updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
                return result ? Ok("Category deleted.") : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
