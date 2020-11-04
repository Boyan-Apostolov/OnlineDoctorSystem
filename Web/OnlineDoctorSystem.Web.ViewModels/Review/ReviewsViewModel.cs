using System.Collections.Generic;
using AutoMapper;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Review
{
    public class ReviewsViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}