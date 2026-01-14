using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=SystemGestionISGA")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        // Auth / People
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        // Academic structure
        public DbSet<Program> Programs { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }

        // ✅ New: Program version per year
        public DbSet<ProgramAcademicYear> ProgramAcademicYears { get; set; }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Semester> Semesters { get; set; }

        // Curriculum
        public DbSet<Module> Modules { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        // Join tables
        public DbSet<SemesterModule> SemesterModules { get; set; }
        public DbSet<ModuleSubject> ModuleSubjects { get; set; }

        // Enrollment
        public DbSet<Inscription> Inscriptions { get; set; }

        // Grades / Teaching
        public DbSet<TeachingUnit> TeachingUnits { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // ✅ ProgramAcademicYear (Program version per AcademicYear)
            // =========================================================

            // Unique index: (ProgramId, AcademicYearId)
            modelBuilder.Entity<ProgramAcademicYear>()
                .Property(x => x.ProgramId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Program_AcYear", 1) { IsUnique = true })
                );

            modelBuilder.Entity<ProgramAcademicYear>()
                .Property(x => x.AcademicYearId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Program_AcYear", 2) { IsUnique = true })
                );

            // ProgramAcademicYear -> Program (catalog)  (NO cascade)
            modelBuilder.Entity<ProgramAcademicYear>()
                .HasRequired(pay => pay.Program)
                .WithMany(p => p.ProgramAcademicYears)   // Program.ProgramAcademicYears
                .HasForeignKey(pay => pay.ProgramId)
                .WillCascadeOnDelete(false);

            // ProgramAcademicYear -> AcademicYear (NO cascade)
            modelBuilder.Entity<ProgramAcademicYear>()
                .HasRequired(pay => pay.AcademicYear)
                .WithMany(y => y.ProgramAcademicYears)   // AcademicYear.ProgramAcademicYears
                .HasForeignKey(pay => pay.AcademicYearId)
                .WillCascadeOnDelete(false);

            // =========================================================
            // ✅ SAFE CASCADE: only for "STRUCTURE" tables
            // =========================================================

            // ✅ ProgramAcademicYear (1) -> Level (many)  (CASCADE ON)
            // Requires: Level.ProgramAcademicYearId + Level.ProgramAcademicYear nav
            modelBuilder.Entity<Level>()
                .HasRequired(l => l.ProgramAcademicYear)
                .WithMany(pay => pay.Levels)  // ProgramAcademicYear.Levels
                .HasForeignKey(l => l.ProgramAcademicYearId)
                .WillCascadeOnDelete(true);

            // Level (1) -> Semester (many)  (CASCADE ON)
            modelBuilder.Entity<Semester>()
                .HasRequired(s => s.Level)
                .WithMany(l => l.Semesters)
                .HasForeignKey(s => s.LevelId)
                .WillCascadeOnDelete(true);

            // Semester (1) -> SemesterModule (many)  (CASCADE ON)
            modelBuilder.Entity<SemesterModule>()
                .HasRequired(sm => sm.Semester)
                .WithMany(s => s.SemesterModules)
                .HasForeignKey(sm => sm.SemesterId)
                .WillCascadeOnDelete(true);

            // Module (1) -> SemesterModule (many)  (NO cascade to reduce cascade paths)
            modelBuilder.Entity<SemesterModule>()
                .HasRequired(sm => sm.Module)
                .WithMany(m => m.SemesterModules)
                .HasForeignKey(sm => sm.ModuleId)
                .WillCascadeOnDelete(false);

            // Module (1) -> ModuleSubject (many)  (CASCADE ON)
            modelBuilder.Entity<ModuleSubject>()
                .HasRequired(ms => ms.Module)
                .WithMany(m => m.ModuleSubjects)
                .HasForeignKey(ms => ms.ModuleId)
                .WillCascadeOnDelete(true);

            // Subject (1) -> ModuleSubject (many)  (NO cascade)
            modelBuilder.Entity<ModuleSubject>()
                .HasRequired(ms => ms.Subject)
                .WithMany(su => su.ModuleSubjects)
                .HasForeignKey(ms => ms.SubjectId)
                .WillCascadeOnDelete(false);

            // =========================================================
            // ✅ User (1) -> Student / Teacher (0/1) shared PK (CASCADE OK)
            // Requires: User.Student + Student.User, User.Teacher + Teacher.User
            // =========================================================

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Student)
                .WithRequired(s => s.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Teacher)
                .WithRequired(t => t.User)
                .WillCascadeOnDelete(true);

            // =========================================================
            // ❌ NO CASCADE: "HISTORY / TRANSACTIONS"
            // =========================================================

            // Inscription -> Student (NO cascade)
            modelBuilder.Entity<Inscription>()
                .HasRequired(i => i.Student)
                .WithMany(s => s.Inscriptions)
                .HasForeignKey(i => i.StudentId)
                .WillCascadeOnDelete(false);

            // Inscription -> AcademicYear (NO cascade)
            //modelBuilder.Entity<Inscription>()
            //    .HasRequired(i => i.AcademicYear)
            //    .WithMany(y => y.Inscriptions)
            //    .HasForeignKey(i => i.AcademicYearId)
            //    .WillCascadeOnDelete(false);

            // ✅ Inscription -> ProgramAcademicYear (NO cascade)
            // Requires: Inscription.ProgramAcademicYearId + nav
            //modelBuilder.Entity<Inscription>()
            //    .HasRequired(i => i.ProgramAcademicYear)
            //    .WithMany(pay => pay.Inscriptions) // ProgramAcademicYear.Inscriptions
            //    .HasForeignKey(i => i.ProgramAcademicYearId)
            //    .WillCascadeOnDelete(false);

            // Inscription -> Level (NO cascade)
            modelBuilder.Entity<Inscription>()
                .HasRequired(i => i.Level)
                .WithMany(l => l.Inscriptions)
                .HasForeignKey(i => i.LevelId)
                .WillCascadeOnDelete(false);

            // TeachingUnit -> AcademicYear (NO cascade)
            modelBuilder.Entity<TeachingUnit>()
                .HasRequired(tu => tu.AcademicYear)
                .WithMany(y => y.TeachingUnits)
                .HasForeignKey(tu => tu.AcademicYearId)
                .WillCascadeOnDelete(false);

            // TeachingUnit -> Semester (NO cascade)
            modelBuilder.Entity<TeachingUnit>()
                .HasRequired(tu => tu.Semester)
                .WithMany(s => s.TeachingUnits)
                .HasForeignKey(tu => tu.SemesterId)
                .WillCascadeOnDelete(false);

            // TeachingUnit -> Teacher (NO cascade)
            modelBuilder.Entity<TeachingUnit>()
                .HasRequired(tu => tu.Teacher)
                .WithMany(t => t.TeachingUnits)
                .HasForeignKey(tu => tu.TeacherId)
                .WillCascadeOnDelete(false);

            // TeachingUnit -> ModuleSubject (NO cascade)
            modelBuilder.Entity<TeachingUnit>()
                .HasRequired(tu => tu.ModuleSubject)
                .WithMany(ms => ms.TeachingUnits)
                .HasForeignKey(tu => tu.ModuleSubjectId)
                .WillCascadeOnDelete(false);

            // Grade -> Inscription (NO cascade)
            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.Inscription)
                .WithMany(i => i.Grades)
                .HasForeignKey(g => g.InscriptionId)
                .WillCascadeOnDelete(false);

            // Grade -> TeachingUnit (NO cascade)
            modelBuilder.Entity<Grade>()
                .HasRequired(g => g.TeachingUnit)
                .WithMany(tu => tu.Grades)
                .HasForeignKey(g => g.TeachingUnitId)
                .WillCascadeOnDelete(false);
        }
    }
}
