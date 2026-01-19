using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.User;


[ApiController]
[Route("user")]
public class Controller : ControllerBase
{
    [HttpGet("{email}")]
    public async Task<ActionResult<FindByEmailResponse>> FindByEmail(
        string email, Repository repository)
    {
        try
        {
            var user = await repository.Get(email);
            return new FindByEmailResponse(user.UserId, user.Username //, user.Email
            );
        }
        catch (Exceptions.NotFound e)
        {
            return NotFound();
        }
    }

    public record FindByEmailResponse(
        [Required] [property: JsonPropertyName("user_id")] int UserId,
        [Required] [property: JsonPropertyName("username")] string Username
        );
}