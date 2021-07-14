using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class Material : RecordBase
    {
        [Required]
        public string Name { get; set; }
        public virtual List<FoodMaterial> FoodMaterials { get; set; }

    }
}
