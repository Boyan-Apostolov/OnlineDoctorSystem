namespace OnlineDoctorSystem.Data.Models
{

    using OnlineDoctorSystem.Data.Common.Models;

    public class PatientDoctor :BaseDeletableModel<int>
    {
        public string DoctorId { get; set; }

        public string PatientId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
