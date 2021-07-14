using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class Category : RecordBase
    {
        [Required]
        public string Name { get; set; }
        public List<Food> Foods { get; set; }
    }
}
