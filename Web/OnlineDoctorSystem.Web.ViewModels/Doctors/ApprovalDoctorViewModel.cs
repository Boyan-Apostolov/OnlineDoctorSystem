namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class ApprovalDoctorViewModel : IMapFrom<Doctor>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Specialty Specialty { get; set; }

        public Town Town { get; set; }

        public double YearsOfPractice { get; set; }

        public bool IsWorkingWithNZOK { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        public string Phone { get; set; }

        public string SmallInfo { get; set; }

        public string WorkingHours { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }
    }
}
