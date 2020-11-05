using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class TownsIndexViewModels
    {
        public IEnumerable<TownsIndexViewModel> Towns { get; set; }
    }
}
