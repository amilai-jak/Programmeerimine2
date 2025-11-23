using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class AssetClass
    {
        [Key]
        public int AssetClassID { get; set; }
        public string Name { get; set; }
    }
}
