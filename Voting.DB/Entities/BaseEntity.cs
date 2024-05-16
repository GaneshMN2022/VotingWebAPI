using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.DB.Entities {
    public class BaseEntity {
        [Timestamp]
        public byte[]? Row_Version { get; set; }
    }
}
