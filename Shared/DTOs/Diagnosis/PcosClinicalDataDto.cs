using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PcosClinicalDataDto
    {
        public double? Age { get; set; }
        public double? BMI { get; set; }
        public double? FSH { get; set; }
        public double? LH { get; set; }
    }
}
