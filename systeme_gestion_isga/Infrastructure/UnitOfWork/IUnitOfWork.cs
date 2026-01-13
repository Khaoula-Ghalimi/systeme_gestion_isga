using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories.Users;
using systeme_gestion_isga.Infrastructure.Repositories.AcademicYears;
using systeme_gestion_isga.Infrastructure.Repositories.Levels;
using systeme_gestion_isga.Infrastructure.Repositories.Modules;
using systeme_gestion_isga.Infrastructure.Repositories.ModuleSubjects;
using systeme_gestion_isga.Infrastructure.Repositories.ProgramAcademicYears;
using systeme_gestion_isga.Infrastructure.Repositories.Programs;
using systeme_gestion_isga.Infrastructure.Repositories.SemesterModules;
using systeme_gestion_isga.Infrastructure.Repositories.Semesters;
using systeme_gestion_isga.Infrastructure.Repositories.Students;
using systeme_gestion_isga.Infrastructure.Repositories.Subjects;
using systeme_gestion_isga.Infrastructure.Repositories.Teachers;

namespace systeme_gestion_isga.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAcademicYearRepository AcademicYears { get; }
        IProgramRepository Programs { get; }
        ILevelRepository Levels { get; }
        ISemesterRepository Semesters { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        IUserRepository Users { get; }

        IProgramAcademicYearRepository ProgramAcademicYears { get; }

        IModuleRepository Modules { get; }
        ISubjectRepository Subjects { get; }

        IModuleSubjectRepository ModuleSubjects { get; }

        ISemesterModuleRepository SemesterModules { get; }

        int Save();
        void Dispose();
    }
}