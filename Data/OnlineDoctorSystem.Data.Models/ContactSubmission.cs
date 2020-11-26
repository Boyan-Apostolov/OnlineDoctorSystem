namespace OnlineDoctorSystem.Data.Models
{
    using OnlineDoctorSystem.Data.Common.Models;

    public class ContactSubmission : BaseModel<int>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }
    }
}
