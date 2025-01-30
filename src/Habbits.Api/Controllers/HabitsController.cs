using Microsoft.AspNetCore.Mvc;

namespace Habbits.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HabitsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateHabit()
    {
        return Ok();
    }
}
