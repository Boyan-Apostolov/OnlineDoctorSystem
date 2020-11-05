using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class TownsIndexViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
