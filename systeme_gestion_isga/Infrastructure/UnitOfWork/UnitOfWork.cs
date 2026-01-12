using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories.Users;
using systeme_gestion_isga.Infrastructure.Database;
using systeme_gestion_isga.Infrastructure.Repositories.AcademicYears;
using systeme_gestion_isga.Infrastructure.Repositories.Levels;
using systeme_gestion_isga.Infrastructure.Repositories.Programs;
using systeme_gestion_isga.Infrastructure.Repositories.Semesters;
using systeme_gestion_isga.Infrastructure.Repositories.Students;
using systeme_gestion_isga.Infrastructure.Repositories.Teachers;

namespace systeme_gestion_isga.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context; // <-- rename to your DbContext class
   
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