using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDoctorSystem.Services.Data.Events
{
    public interface IEventsService
    {
        Task<bool> DeleteEventByIdAsync(int id);
    }
}
