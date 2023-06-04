using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zadanie.Models.DTO
{
    public class GetPrescriptionDto
    {
        public string Medicament { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
    }
}
