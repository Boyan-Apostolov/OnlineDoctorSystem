using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDoctorSystem.Services
{
    public interface IDoctorScraperService
    {
        Task<int> Import(int pages = 1);
    }
}
