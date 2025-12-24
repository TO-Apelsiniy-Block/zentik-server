using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.User;


[ApiController]
[Route("user")]
public class UserController : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<FindByEmailResponse>> FindByEmail(string email)
    {
        return new FindByEmailResponse(1);
    }

    public record FindByEmailResponse(
        [Required] [property: JsonPropertyName("message")] int qwe);
}