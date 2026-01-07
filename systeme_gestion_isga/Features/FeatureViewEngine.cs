using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace systeme_gestion_isga.Features
{
    public class FeatureViewEngine : RazorViewEngine
    {
        public FeatureViewEngine()
        {
            ViewLocationFormats = new[]
            {
            // Feature-based
            "~/Features/{1}/Views/{0}.cshtml",
            "~/Features/{1}/Views/Shared/{0}.cshtml",

            // Fallback to classic MVC
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.cshtml"
        };

            PartialViewLocationFormats = ViewLocationFormats;
        }
    }
}