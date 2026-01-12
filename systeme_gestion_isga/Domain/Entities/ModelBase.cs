using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class ModelBase
    {
        protected ModelBase()
        {
            var now = DateTime.UtcNow; 
            CreatedAt = now;
            UpdatedAt = now;
        }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Touch() => UpdatedAt = DateTime.UtcNow;
    }
}