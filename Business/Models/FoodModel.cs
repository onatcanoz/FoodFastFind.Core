using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AppCore.Records.Bases;
using Entities.Entities;

namespace Business.Models
{
    public class FoodModel : RecordBase
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Categories")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Recipes")]
        public string Detail { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("ingredients in")]
        public string RecipesMaterials { get; set; }

        [DisplayName("Photo")]
        public string PhotoURL { get; set; }

        [DisplayName("Video")]
        public string VideoURL { get; set; }
        [Required]
        [DisplayName("Cook Time")]
        public int CookTime { get; set; }
        [Required]
        [DisplayName("Person Number")]
        public int PersonNumber { get; set; }

        public CategoryModel Category { get; set; }

        public List<MaterialModel> Materials { get; set; }

        [DisplayName("Materials")]
        public string MaterialText => Materials == null || Materials.Count == 0 ? "" : string.Join("<br />", Materials.Select(m => m.Name));

        [DisplayName("Materials")]
        public List<int> MaterialsIds { get; set; }
    }
}
