using System.ComponentModel.DataAnnotations.Schema;

namespace ZenticServer.Base;

public class Model
{
    // Базовая модель от которой необходимо наследоваться
    
    // ВАЖНО!!! При создании новой таблицы
    // необходимо повесить на неё тригер для обновления updated_at
    
    // Важно! Производные модели от Message не наследуются от Base.Model
    
    [Column("created_at")]
    public DateTime CreatedAt;
    
    [Column("updated_at")]
    public DateTime UpdatedAt;
    
    // TODO разобраться с таймзонами
}