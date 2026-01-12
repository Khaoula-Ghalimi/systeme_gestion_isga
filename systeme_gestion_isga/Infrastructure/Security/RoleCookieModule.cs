using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace systeme_gestion_isga.Infrastructure.Security
{
    public class RoleCookieModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += (sender, e) =>
            {
                if(HttpContext.Current.User?.Identity is FormsIdentity id)
                {
                    var ticket = id.Ticket;
                    var roles = ticket.UserData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    HttpContext.Current.User = new GenericPrincipal(id, roles);
                }
            };
        }
    }
}