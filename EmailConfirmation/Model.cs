using System.ComponentModel.DataAnnotations;

namespace ZenticServer.EmailConfirmation;

public class Model : Base.Model
{
    public int Code { get; set; }
    
    [MaxLength(64)]
    public string Email { get; set; }
    
    public int DeviceId { get; set; }
}