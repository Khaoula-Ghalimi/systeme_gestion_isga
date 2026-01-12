using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.AcademicYear.ViewModels
{
    public class ProgramAcademicYearVM
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DurationInYears { get; set; }
    }
}