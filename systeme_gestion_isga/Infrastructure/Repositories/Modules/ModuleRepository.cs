using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;

namespace systeme_gestion_isga.Infrastructure.Repositories.Modules
{
    public class ModuleRepository : GenericRepository<Domain.Entities.Module>, IModuleRepository
    {
        public ModuleRepository(Database.AppDbContext context) : base(context)
        {
        }
        public ModuleRepository() : this(new Database.AppDbContext())
        {
        }
    
    }
}