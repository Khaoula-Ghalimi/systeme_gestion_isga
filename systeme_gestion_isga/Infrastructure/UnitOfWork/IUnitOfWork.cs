using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories.Users;
using systeme_gestion_isga.Infrastructure.Repositories.AcademicYears;
using systeme_gestion_isga.Infrastructure.Repositories.Levels;
using systeme_gestion_isga.Infrastructure.Repositories.Programs;
using systeme_gestion_isga.Infrastructure.Repositories.Semesters;
using systeme_gestion_isga.Infrastructure.Repositories.Students;
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

        int Save();
        void Dispose();
    }
}