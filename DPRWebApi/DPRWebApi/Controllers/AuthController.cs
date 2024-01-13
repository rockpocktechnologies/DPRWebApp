using Microsoft.AspNetCore.Mvc;

namespace DPRWebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Validate login credentials (replace this with your actual authentication logic)
            if (IsValidLogin(loginModel))
            {
                // In a real-world scenario, you would generate and return a token here
                // For simplicity, we'll return a success message
                return Ok(new { Message = "Login successful" });
            }

            // Return unauthorized status if login fails
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        // Replace this method with your actual authentication logic
        private bool IsValidLogin(LoginModel loginModel)
        {
            // Example: Check if the username and password match a user in the database
            return loginModel.Username == "demo" && loginModel.Password == "password";
        }

        // LoginModel.cs
        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }
}
