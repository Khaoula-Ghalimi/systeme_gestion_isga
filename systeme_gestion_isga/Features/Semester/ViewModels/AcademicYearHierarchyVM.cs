using System;
using System.Collections.Generic;

namespace systeme_gestion_isga.Features.Semester.ViewModels
{
    public class AcademicYearsTreeVM
    {
        public List<AcademicYearNodeVM> AcademicYears { get; set; } = new List<AcademicYearNodeVM>();
    }

    public class AcademicYearNodeVM
    {
        public int AcademicYearId { get; set; }
        public string AcademicYearName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public List<ProgramNodeVM> Programs { get; set; } = new List<ProgramNodeVM>();
    }

    public class ProgramNodeVM
    {
        public int ProgramAcademicYearId { get; set; }
        public int ProgramId { get; set; }

        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public int? DurationInYears { get; set; }
        public bool IsActive { get; set; }

        public List<LevelNodeVM> Levels { get; set; } = new List<LevelNodeVM>();
    }

    public class LevelNodeVM
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        public List<SemesterNodeVM> Semesters { get; set; } = new List<SemesterNodeVM>();
    }

    public class SemesterNodeVM
    {
        public int SemesterId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
}
