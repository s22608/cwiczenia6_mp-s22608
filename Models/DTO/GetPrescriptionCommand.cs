using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zadanie.Models.DTO
{
    public class GetPrescriptionCommand
    {
        public int IdDoctor { get; set; }
        public int IdPatient { get; set; }
        public string Medicament { get; set; }
    }
}
