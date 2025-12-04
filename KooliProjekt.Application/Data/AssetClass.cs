using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class AssetClass
    {
        [Key]
        public int AssetClassID { get; set; }
        
        [Required(ErrorMessage = "Vara klassi nimi on kohustuslik")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nimi peab olema 1-100 märki")]
        public string Name { get; set; }
    }
}
