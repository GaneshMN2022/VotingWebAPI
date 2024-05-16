using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.DB.Entities {
    public class VotersEntity : BaseEntity {
        [Key]
        public int VoterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? VotedOn { get; set; }
        public char HasVoted { get; set; } = 'x';
        public bool? IsDeleted { get; set; }
    }
}
