using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.AcademicYear.ViewModels;

namespace systeme_gestion_isga.Infrastructure.Repositories.ProgramAcademicYears
{
    public interface IProgramAcademicYearRepository : IGenericRepository<Domain.Entities.ProgramAcademicYear>
    {
        List<ProgramAcademicYearVM> GetByAcademicYear(int academicYearId);
        List<Program> GetExceptAcademicYear(int academicYearId);
    }
}
