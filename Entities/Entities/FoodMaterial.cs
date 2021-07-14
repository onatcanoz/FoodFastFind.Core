using System;
using System.Collections.Generic;
using System.Text;
using AppCore.Records.Bases;

namespace Entities.Entities
{
    public class FoodMaterial : RecordBase
    {
        public int FoodId { get; set; }
        public virtual Food Food { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

    }
}
