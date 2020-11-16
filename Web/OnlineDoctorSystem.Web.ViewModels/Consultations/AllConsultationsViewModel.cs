using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    public class AllConsultationsViewModel
    {
        public IEnumerable<ConsultationViewModel> Consultations { get; set; }
    }
}
