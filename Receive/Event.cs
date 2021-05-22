using System;
using System.Collections.Generic;
using System.Text;

namespace Receive
{
    class Event
    {
        public int ID { get; set; }
        public string Opertation { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
