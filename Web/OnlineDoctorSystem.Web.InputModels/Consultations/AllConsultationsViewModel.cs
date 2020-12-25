namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    using System.Collections.Generic;

    public class AllConsultationsViewModel
    {
        public IEnumerable<ConsultationViewModel> Consultations { get; set; }
    }
}
