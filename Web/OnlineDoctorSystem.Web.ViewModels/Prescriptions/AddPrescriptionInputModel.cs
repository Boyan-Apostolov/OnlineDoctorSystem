namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class AddPrescriptionInputModel : IMapFrom<Prescription>
    {
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public string Quantity { get; set; }

        public string MedicamentName { get; set; }

        public string Instructions { get; set; }
    }
}
