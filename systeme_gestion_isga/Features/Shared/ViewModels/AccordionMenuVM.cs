using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.Shared.ViewModels
{
    public class AccordionMenuVM
    {
        public string Id { get; set; }          
        public string TriggerText { get; set; } 
        public string TriggerIcon { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }
        public List<MenuItemVM> Items { get; set; }
        public string ParentAccordionId { get; set; } = "sidebarAccordion";
    }
}