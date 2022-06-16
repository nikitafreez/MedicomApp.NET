using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicomApp.NET.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public DateTime TreatmentDate { get; set; }
        public float TreatmentSum { get; set; }
        public string DiseaseName { get; set; }
        public string Patient { get; set; }
        public string Worker { get; set; }
    }
}
