namespace OnlineDoctorSystem.Data.Models
{

    using OnlineDoctorSystem.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
