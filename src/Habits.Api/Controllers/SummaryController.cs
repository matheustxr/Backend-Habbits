using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SummaryController : ControllerBase
{
    
}

