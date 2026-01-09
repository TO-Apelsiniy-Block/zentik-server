using System.ComponentModel.DataAnnotations.Schema;

namespace ZenticServer.Base;

public class Model
{
    // Базовая модель от которой необходимо наследоваться
    [Column("created_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt;
    
    [Column("updated_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt;
}