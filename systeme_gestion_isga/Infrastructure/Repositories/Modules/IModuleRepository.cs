using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using system_gestion_isga.Infrastructure.Repositories;

namespace systeme_gestion_isga.Infrastructure.Repositories.Modules
{
    public interface IModuleRepository : IGenericRepository<Domain.Entities.Module>
    {
    }
}
