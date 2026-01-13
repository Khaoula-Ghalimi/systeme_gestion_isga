using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Infrastructure.Database;

namespace systeme_gestion_isga.Infrastructure.Repositories.SemesterModules
{
    public class SemesterModuleRepository : GenericRepository<Domain.Entities.SemesterModule>, ISemesterModuleRepository
    {
        public SemesterModuleRepository(AppDbContext context) : base(context)
        {
        }

        public SemesterModuleRepository() : this(new Database.AppDbContext())
        {
        }
    }
}