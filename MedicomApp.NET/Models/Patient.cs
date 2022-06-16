using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicomApp.NET.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surmame { get; set; }
        public string Patronymic { get; set; }
        public string OMS { get; set; }
        public string PassNum { get; set; }
        public string PassSeria { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string FIO
        {
            get => $"{Surmame} {Name} {Patronymic}";
        }
    }
}
