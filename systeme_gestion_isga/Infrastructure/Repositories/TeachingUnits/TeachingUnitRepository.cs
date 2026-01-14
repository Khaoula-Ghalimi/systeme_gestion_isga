using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Infrastructure.Database;

namespace systeme_gestion_isga.Infrastructure.Repositories.TeachingUnits
{
    public class TeachingUnitRepository : GenericRepository<Domain.Entities.TeachingUnit>, ITeachingUnitRepository
    {
        public TeachingUnitRepository(AppDbContext context) : base(context)
        {
        }
        public TeachingUnitRepository() : this(new Database.AppDbContext())
        {
        }
    }
}