using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OnlineDoctorSystem.Web.ViewModels.Chat;

namespace OnlineDoctorSystem.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("NewMessage", new Message(){User = this.Context.User.Identity.Name, Text = message});
        }
    }
}
