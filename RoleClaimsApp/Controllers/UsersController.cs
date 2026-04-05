using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RoleClaimsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    // Fix: Changed "role-base" to "role-based" to match your URI
    [HttpGet("role-based")] 
    [Authorize(Roles = "Admin")]
    public IActionResult GetUsersByRole()
    {
        return Ok(new { Message = "Access granted: You have the role of Admin."});
    }

    [HttpGet("claims-based")]
    [Authorize(Policy = "RequireITDepartment")]
    public IActionResult GetUsersByClaims()
    {
        return Ok(new { Message = "Access granted: You are part of the IT Department."});
    }
} // Ensure this closing brace exists