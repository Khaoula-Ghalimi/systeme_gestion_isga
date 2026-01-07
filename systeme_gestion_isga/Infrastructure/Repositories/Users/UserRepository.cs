using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Infrastructure.Database;


namespace system_gestion_isga.Infrastructure.Repositories.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository() : base() { }
        public UserRepository(AppDbContext context) : base(context) { }

        public bool Exists(string username)
        {
            return _dbSet.Any(u => u.Username.ToLower() == username.ToLower());

        }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        }   

        public User Login(string username, string passwordHash)
        {
            return _dbSet.FirstOrDefault(u =>
                u.Username.ToLower() == username.ToLower() &&
                u.PasswordHash == passwordHash
            );
        }
    }
}