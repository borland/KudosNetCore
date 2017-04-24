using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KudosNetCore.Model
{
    [Flags]
    public enum UserFlags
    {
        None = 0,
        Admin = 1
    }

    // only admins need to log on, if they do they get a record here
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public byte[] PasswordSalt { get; set; }
        
        public byte[] PasswordHash { get; set; }

        public UserFlags Flags { get; set; }

        public List<UserTeam> UserTeams { get; set; }
    }

    // m2m join table for users who can administer teams
    public class UserTeam
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
