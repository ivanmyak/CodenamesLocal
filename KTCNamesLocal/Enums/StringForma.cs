using System.ComponentModel.DataAnnotations;

namespace CodenamesClean.Enums
{
    public enum StringForma
    {
        [Display(Name = "Слова")]
        Words,
        [Display(Name = "Прилагательные")]
        Adjectives,
        [Display(Name = "Существительные")]
        Nouns

    };
}
