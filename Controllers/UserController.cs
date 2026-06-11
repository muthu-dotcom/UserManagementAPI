using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>();

        //GET all users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        //GET user by ID
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound("User not found");

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //Validate User
        private bool IsValidUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                return false;

            if (string.IsNullOrWhiteSpace(user.Email))
                return false;

            // basic email check
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(user.Email, emailPattern);
        }

        //POST add user
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            try
            {
                if (!IsValidUser(user))
                    return BadRequest("Invalid user data");

                user.Id = users.Count + 1;
                users.Add(user);

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error adding user");
            }
        }

        //PUT update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            try
            {
                if (!IsValidUser(updatedUser))
                    return BadRequest("Invalid user data");

                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound("User not found");

                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating user");
            }
        }

        //DELETE user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return NotFound("User not found");

                users.Remove(user);

                return Ok("User deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error deleting user");
            }
        }
    }
}
