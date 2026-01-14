using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Repositories;

namespace systeme_gestion_isga.Infrastructure.Repositories.Inscription
{
    public class InscriptionRepository : GenericRepository<Domain.Entities.Inscription>, IInscriptionRepository
    {
        public InscriptionRepository(Database.AppDbContext context) : base(context)
        {
        }
        public InscriptionRepository() : this(new Database.AppDbContext())
        {
        }
    }
}