using System.ComponentModel.DataAnnotations;

namespace CodenamesClean.Enums
{
    public enum ClassicRole
    {
        [Display(Name = "Красный")]
        RedAgent,
        [Display(Name = "Синий")]
        BlueAgent,
        [Display(Name = "Нейтрал")]
        Neutral,
        [Display(Name = "Убийца")]
        Assassin
    }
}
