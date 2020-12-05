using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Statistics
{
    public class StatsViewModel
    {
        public int DoctorsCount { get; set; }

        public int PatientsCount { get; set; }

        public int ConsultationsCount { get; set; }

        public int SpecialtiesCount { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<TownViewModel> Towns { get; set; }
    }
}
