using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Infrastructure.Repositories.Levels
{
    public class LevelRepository : GenericRepository<Level>, ILevelRepository
    {
        

        public LevelRepository(Database.AppDbContext context) : base(context)
        {
        }

        public List<Level> GetByProgramAcademicYearId(int programAcademicYearId)
        {
            return _dbSet
                .Where(l => l.ProgramAcademicYearId == programAcademicYearId)
                .OrderBy(l => l.Order)
                .ToList();
        }
    }
}