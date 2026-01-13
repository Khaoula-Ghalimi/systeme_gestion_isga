using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.AcademicYear.ViewModels;
using systeme_gestion_isga.Infrastructure.Database;

namespace systeme_gestion_isga.Infrastructure.Repositories.ProgramAcademicYears
{
    public class ProgramAcademicYearRepository : GenericRepository<Domain.Entities.ProgramAcademicYear>, IProgramAcademicYearRepository
    {
        public ProgramAcademicYearRepository(AppDbContext context) : base(context)
        {
        }

        public List<ProgramAcademicYearVM> GetByAcademicYear(int academicYearId)
        {
            return _context.ProgramAcademicYears
                .Where(pay => pay.AcademicYearId == academicYearId)
                .Select(pay => new ProgramAcademicYearVM
                {
                    Id = pay.Id,
                    ProgramId = pay.Program.Id,
                    Name = pay.Program.Name,
                    Code = pay.Program.Code,
                    DurationInYearsOverride = pay.DurationInYearsOverride
                                      ?? pay.Program.DurationInYears,
                    IsActive = pay.IsActive
                })
                .OrderBy(p => p.ProgramId)
                .ToList();
        }

        public List<Program> GetExceptAcademicYear(int academicYearId)
        {
            var programIdsInYear = _context.ProgramAcademicYears
                .Where(pay => pay.AcademicYearId == academicYearId)
                .Select(pay => pay.ProgramId);

            return _context.Programs
                .Where(p => !programIdsInYear.Contains(p.Id))
                .OrderBy(p => p.Id)
                .ToList();
        }

    }
}