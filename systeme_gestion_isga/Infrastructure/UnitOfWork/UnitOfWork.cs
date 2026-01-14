using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories.Users;
using systeme_gestion_isga.Infrastructure.Database;
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
using systeme_gestion_isga.Infrastructure.Repositories.TeachingUnits;

namespace systeme_gestion_isga.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context; 
   
        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            AcademicYears = new AcademicYearRepository(_context);
            Programs = new ProgramRepository(_context);
            Levels = new LevelRepository(_context);
            Semesters = new SemesterRepository(_context);
            Students = new StudentRepository(_context);
            Teachers = new TeacherRepository(_context);
            Users = new UserRepository(_context);
            ProgramAcademicYears = new ProgramAcademicYearRepository(_context);
            Modules = new ModuleRepository(_context);
            Subjects = new SubjectRepository(_context);
            ModuleSubjects = new ModuleSubjectRepository(_context);
            SemesterModules = new SemesterModuleRepository(_context);
            TeachingUnits = new TeachingUnitRepository(_context);
        }

        // Optional parameterless ctor (if you’re not using DI yet)
        public UnitOfWork() : this(new AppDbContext())
        {
        }

        public IAcademicYearRepository AcademicYears { get; private set; }
        public IProgramRepository Programs { get; private set; }
        public ILevelRepository Levels { get; private set; }
        public ISemesterRepository Semesters { get; private set; }
        public IStudentRepository Students { get; private set; }
        public ITeacherRepository Teachers { get; private set; }
        public IUserRepository Users { get; private set; }
        public IProgramAcademicYearRepository ProgramAcademicYears { get; private set; }

        public IModuleRepository Modules { get; private set; }
        public ISubjectRepository Subjects { get; private set; }

        public IModuleSubjectRepository ModuleSubjects { get; private set; }

        public ISemesterModuleRepository SemesterModules { get; private set; }

        public ITeachingUnitRepository TeachingUnits { get; private set; }


        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}