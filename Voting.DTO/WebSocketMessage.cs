using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voting.DTO {
    public class WebSocketMessage {
        public string Type { get; set; }
        public object Payload { get; set; }
    }
}
