using Microsoft.AspNetCore.Mvc;

namespace ZenticServer.Message;

[ApiController]
[Route("api/[controller]")]
public class MessageController
{
    [HttpPost("{userId}")]
    public async Task WriteMessage()
    {
        
    }
    
}