using System.ComponentModel.DataAnnotations;

namespace CodenamesClean.Enums
{
    public enum Team
    {
        [Display(Name = "Отсутствует")]
        None,
        [Display(Name = "Красные")]
        Red,
        [Display(Name = "Синие")]
        Blue
    }
}
