using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class Food : RecordBase
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Detail { get; set; }
        public string PhotoURL { get; set; }
        [Required]
        [StringLength(1000)]
        public string RecipesMaterials { get; set; }
        public string VideoURL { get; set; }
        [Required]
        public int CookTime { get; set; }
        [Required]
        public int PersonNumber { get; set; }
        public Category Category { get; set; }
        public virtual List<FoodMaterial> FoodMaterials { get; set; }
    }
}
