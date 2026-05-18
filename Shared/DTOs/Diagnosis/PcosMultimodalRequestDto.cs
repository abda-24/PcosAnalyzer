using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Diagnosis
{
    public class PcosMultimodalRequestDto
    {
        public string? ClinicalData { get; set; }
        public IFormFile? UltrasoundImage { get; set; }
    }
}
