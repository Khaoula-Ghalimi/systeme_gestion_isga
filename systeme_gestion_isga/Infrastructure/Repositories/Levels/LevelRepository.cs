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
        //public List<Level> GetByProgramId(int programId)
        //{
        //    return _dbSet.Where(l => l.ProgramId == programId).OrderBy(l => l.Order).ToList();
        //}

        public LevelRepository(Database.AppDbContext context) : base(context)
        {
        }

        public List<Level> GetByProgramId(int programId)
        {
            throw new NotImplementedException();
        }
    }
}