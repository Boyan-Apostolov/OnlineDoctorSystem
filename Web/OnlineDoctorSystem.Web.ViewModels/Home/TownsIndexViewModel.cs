namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class TownsIndexViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
