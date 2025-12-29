using System.ComponentModel.DataAnnotations;

namespace ZenticServer.Chat;

public record Model
(
    [Required] string Name,
    [Required] string Type); // ЛС, группа