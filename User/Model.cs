using System.ComponentModel.DataAnnotations;

namespace ZenticServer.User;

public record Model
(
    [Required] int UserId,
    [Required] string Username,
    [Required] string Email);