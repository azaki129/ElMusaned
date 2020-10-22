using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum
{
    public struct ValidMsg
    {
        public bool IsValid { get; set; }
        public string Msg { get; set; }
        public MessageType Msg_type { get; set; }
        public long EntityId { get; set; }
    }

}
