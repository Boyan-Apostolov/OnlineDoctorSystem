namespace OnlineDoctorSystem.Web.ViewModels.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class TownViewModel : IMapFrom<Town>
    {
        public string Name { get; set; }

        public int DoctorsCount { get; set; }
    }
}
