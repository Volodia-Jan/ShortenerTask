using Shortener.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.RepositoryContracts;

public interface IUsersRepository
{
    Task<bool> SaveUser(ApplicationUser applicationUser, string password, string roleName);

    Task<bool> SignIn(string email, string password);

    Task SignIn(ApplicationUser user);

    Task<ApplicationUser?> FindUserByEmail(string email);

    Task<bool> IsUserHasRole(string email, string roleName);

    Task SignOut();
}