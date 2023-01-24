using Microsoft.AspNetCore.Mvc;
using Latihan.DTO;
using Latihan.Models;
using Latihan.Services;
using Latihan.Utils;

namespace Latihan.Controllers
{
    [Utils.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IHash _hash;
        
        private static readonly HttpClient Client = new HttpClient();
        
        public UserController(
            IUserService userService,
            IHash hash
        )
        {
            _userService = userService;
            _hash = hash;
        }
        
        [HttpGet]
        [Authorize(Role.ADMIN)]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userService.GetUser());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserRequest addUserRequest)
        {
            var password = await _hash.GenerateRandomString(new ParamsRandPassword()
            {
                LowerLength = 2,
                UpperLength = 2,
                NumLength = 2,
                SpecialLength = 2,
            });

            Register a = default;
            a.Name = addUserRequest.Name;
            a.Email = addUserRequest.Email;
            a.Password = password.hashed;
            
            var addUser = await _userService.CreateUser(a);

            return Ok(addUser);
        }
        
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, AddUserRequest updateUserRequest)
        {
            var updateUser = await _userService.UpdateUser(id, updateUserRequest);

            return Ok(updateUser);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> DetailUser([FromRoute] Guid id)
        {
            var getUser = await _userService.GetUserById(id);

            return Ok(getUser);
        }
        
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var userFind = await _userService.DeleteUser(id);

            return Ok(userFind);
        }
    }
}
