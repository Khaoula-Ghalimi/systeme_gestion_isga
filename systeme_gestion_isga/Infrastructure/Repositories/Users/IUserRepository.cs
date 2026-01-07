using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using systeme_gestion_isga.Domain.Entities;

namespace system_gestion_isga.Infrastructure.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetByUsername(string username);
        User Login(string username, string passwordHash);
        bool Exists(string username);
    }
}
