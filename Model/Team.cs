using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KudosNetCore.Model
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Kudo> Kudos { get; set; }

        public List<UserTeam> UserTeams { get; set; }
    }
}
