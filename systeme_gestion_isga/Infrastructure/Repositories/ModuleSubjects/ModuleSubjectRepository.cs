using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Infrastructure.Repositories.ModuleSubjects
{
    public class ModuleSubjectRepository : GenericRepository<ModuleSubject>, IModuleSubjectRepository
    {
        public ModuleSubjectRepository(Database.AppDbContext context) : base(context)
        {
        }
        public ModuleSubjectRepository() : this(new Database.AppDbContext())
        {
        }

    }
}