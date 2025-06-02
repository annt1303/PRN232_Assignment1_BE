
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
    //[Authorize(Roles = "Admin")]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _accountService;
        public SystemAccountController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10)
    => Ok(await _accountService.GetAllAccounts(page, size));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _accountService.GetAccountById(id);
            return account == null ? NotFound() : Ok(account);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return Ok(await _accountService.SearchAccountsByNameAsync(name, page, size));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemAccountRequest dto)
        {
            var created = await _accountService.CreateAccount(dto);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SystemAccountDTO dto)
        {
            var success = await _accountService.UpdateAccount(dto);
            return success ? Ok("Updated") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var result = await _accountService.DeleteAccount(id);
                return result ? Ok("Deleted") : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
