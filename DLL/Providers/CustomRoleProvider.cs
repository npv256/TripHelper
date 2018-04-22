using System;
using System.Linq;
using System.Web.Security;
using DLL.EF;
using DLL.Entities;

namespace DLL.Providers
{
    public class CustomRoleProvider : RoleProvider
    {

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            using (TripContext db = new TripContext("TripContext"))
            {
                User user = db.Users.FirstOrDefault(u => u.Email == username);

                if (user != null)
                {
                    roles = new string[] { user.Role };
                }

                return roles;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}