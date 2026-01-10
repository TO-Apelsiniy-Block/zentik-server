using System.ComponentModel.DataAnnotations;

namespace ZenticServer.EmailConfirmation;

public class Model : Base.Model
{
    // TODO убрать DeviceId, сделать нормальный id, для решения проблем с:
    // попытались создать (ошибка), в другом потоке удалили, попытались обновить (ошибка, уже фатальная) 
    public int Code { get; set; }
    
    [MaxLength(64)]
    public string Email { get; set; }
    
    public int DeviceId { get; set; }
}