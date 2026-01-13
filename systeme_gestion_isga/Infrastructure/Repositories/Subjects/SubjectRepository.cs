using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;

namespace systeme_gestion_isga.Infrastructure.Repositories.Subjects
{
    public class SubjectRepository : GenericRepository<Domain.Entities.Subject>, ISubjectRepository
    {
        public SubjectRepository(Database.AppDbContext context) : base(context)
        {
        }
        public SubjectRepository() : this(new Database.AppDbContext())
        {
        }
    }
}