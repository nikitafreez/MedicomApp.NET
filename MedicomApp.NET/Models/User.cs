using System;
using System.Collections.Generic;

namespace MedicomApp.NET.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime LastEnter { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }

        public User(int id, DateTime lastEnter, string password, string userName)
        {
            Id = id;
            LastEnter = lastEnter;
            Password = password;
            UserName = userName;

            List<Role> roles = new List<Role>();
            string roleName = "UNDEFINED";
            foreach (Role role in roles)
            {
                if (role.User_Id == id)
                    roleName = role.RoleName;
            }
            RoleName = roleName;
        }

        public User()
        {
        }
    }
}
