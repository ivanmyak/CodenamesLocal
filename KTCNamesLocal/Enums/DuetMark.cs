using System.ComponentModel.DataAnnotations;

namespace CodenamesClean.Enums
{
    public enum DuetMark
    {
        [Display(Name = "Нейтрал")]
        Neutral,
        [Display(Name = "Правильный")]
        Green,
        [Display(Name = "Убийца")]
        Assassin
    }
}
