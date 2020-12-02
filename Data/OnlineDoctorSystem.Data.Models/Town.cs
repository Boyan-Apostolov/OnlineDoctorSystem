using System.Collections.Generic;

namespace OnlineDoctorSystem.Data.Models
{

    using OnlineDoctorSystem.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public Town()
        {
            this.Doctors = new HashSet<Doctor>();
        }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
