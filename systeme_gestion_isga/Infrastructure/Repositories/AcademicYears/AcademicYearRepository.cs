using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Infrastructure.Database;

namespace systeme_gestion_isga.Infrastructure.Repositories.AcademicYears
{
    public class AcademicYearRepository : GenericRepository<AcademicYear>, IAcademicYearRepository
    {
        public AcademicYearRepository(AppDbContext context) : base(context)
        {
        }
        public AcademicYearRepository() : this(new AppDbContext())
        {
        }

        
    }
}