using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Infrastructure.Repositories.Semesters
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public List<Semester> GetByLevelId(int levelId)
        {
            return _dbSet.Where(s => s.LevelId == levelId).OrderBy(s => s.Order).ToList();
        }
    }
}