using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace BusinessManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserApiController : ControllerBase
    {
        protected ResponseDto<User> _responseDto;
        private readonly ApplicationDBContext _context;

        public UserApiController(ApplicationDBContext context)
        {
            _responseDto = new ResponseDto<User>();
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "User not found.";
                return NotFound(_responseDto);
            }
            else
            {
                _responseDto.Data = user;
                return Ok(_responseDto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "No users found.";
                return NotFound(_responseDto);
            }
            else
            {
                _responseDto.Datas = users;
                return Ok(_responseDto);
            }
        }

        [HttpGet("{status}")]
        public async Task<IActionResult> GetAllActiveUsers(bool status)
        {
            var users = await _context.Users.Where(u => u.Status == status).ToListAsync();
            if (users == null || users.Count == 0)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "No users found.";
                return NotFound(_responseDto);
            }
            else
            {
                _responseDto.Datas = users;
                return Ok(_responseDto);
            }
        }

        [HttpGet("{status}")]
        public async Task<IActionResult> GetAllActiveUsersByString(string status)
        {
            bool stat = status.ToLower() == "active" ? true : false;
            var users = await _context.Users.Where(u => u.Status == stat).ToListAsync();
            if (users == null || users.Count == 0)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "No users found.";
                return NotFound(_responseDto);
            }
            else
            {
                _responseDto.Datas = users;
                return Ok(_responseDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User? user)
        {
            if (user == null)
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "Invalid user data.";
                return BadRequest(_responseDto);
            }
            else
            {
                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    _responseDto.Data = user;
                    return Ok(_responseDto);
                }
                catch (Exception ex)
                {

                    _responseDto.StatusCode = HttpStatusCode.BadRequest;
                    _responseDto.Message = ex.InnerException.Message;
                    return BadRequest(_responseDto);
                }
                
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "Invalid user data.";
                return BadRequest(_responseDto);
            }

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "User not found.";
                return NotFound(_responseDto);
            }

            // Update user properties
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Gender = user.Gender;
            existingUser.Address = user.Address;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Occupation = user.Occupation;
            existingUser.Status = user.Status;
            existingUser.FacebookLink = user.FacebookLink;
            existingUser.InstagramLink = user.InstagramLink;
            existingUser.TiktokLink = user.TiktokLink;
            existingUser.ProfilePictureLink = user.ProfilePictureLink;
            existingUser.Skills = user.Skills;
            existingUser.Notes = user.Notes;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            _responseDto.Data = existingUser;
            return Ok(_responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "Invalid user data.";
                return BadRequest(_responseDto);
            }

            if (id != user.Id)
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "User ID mismatch.";
                return BadRequest(_responseDto);
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "User not found.";
                return NotFound(_responseDto);
            }

            // Update all user properties
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Gender = user.Gender;
            existingUser.Address = user.Address;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Occupation = user.Occupation;
            existingUser.Status = user.Status;
            existingUser.FacebookLink = user.FacebookLink;
            existingUser.InstagramLink = user.InstagramLink;
            existingUser.TiktokLink = user.TiktokLink;
            existingUser.ProfilePictureLink = user.ProfilePictureLink;
            existingUser.Skills = user.Skills;
            existingUser.Notes = user.Notes;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            _responseDto.Data = existingUser;
            return Ok(_responseDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "Invalid user data.";
                return BadRequest(_responseDto);
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "User not found.";
                return NotFound(_responseDto);
            }

            // Partial update
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;


            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            _responseDto.Message = "User patched successfully.";
            _responseDto.Data = existingUser;
            return Ok(_responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "User not found.";
                return NotFound(_responseDto);
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            _responseDto.StatusCode = HttpStatusCode.OK;
            _responseDto.Message = "User deleted successfully.";
            _responseDto.Data = existingUser;
            return Ok(_responseDto);
        }
    }
}
