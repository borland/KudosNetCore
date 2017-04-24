using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KudosNetCore.Model
{
    /// <summary>
    /// A Kudo isn't really a thing. OR IS IT?!?!?!
    /// http://www.dailywritingtips.com/kudo-vs-kudos/
    /// </summary>
    public class Kudo
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(100)]
        public string ForWho { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public string Message { get; set; }

        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }
    }
}
