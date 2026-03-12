using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class AssetClass : Entity
    {
        [Required(ErrorMessage = "Vara klassi nimi on kohustuslik")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nimi peab olema 1-100 m�rki")]
        public string Name { get; set; }
    }
}
