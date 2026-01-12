using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.Shared.ViewModels
{
    public class MenuItemVM
    {
        public string Text { get; set; }
        public string Icon { get; set; }  
        public string Controller { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }
    }
}