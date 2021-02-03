using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Contacts
{
    public class DoctorEmailViewModel
    {
        public string DoctorId { get; set; }

        public string DoctorEmail { get; set; }

        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public string PatientEmail { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
