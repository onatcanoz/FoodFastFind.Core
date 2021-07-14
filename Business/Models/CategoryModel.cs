using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AppCore.Records.Bases;


namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        [Required]
        public string Name { get; set; }

        public List<FoodModel> Foods { get; set; }

        [DisplayName("Foods")]
        public string FoodsText => Foods == null || Foods.Count == 0 ? "ııIıı" : string.Join(" ", Foods.Select(f => f.Name));
    }
}
