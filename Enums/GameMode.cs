using System.ComponentModel.DataAnnotations;

namespace CodenamesClean.Enums
{
    public enum GameMode
    {
        [Display(Name = "Классический")]
        Classic,
        [Display(Name = "Дуэт")]
        Duet
    }
}
