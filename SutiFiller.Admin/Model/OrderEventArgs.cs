using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutiFiller.Admin.Model
{
    public class OrderEventArgs : EventArgs
    {
        public Int32 OrderId { get; set; }
    }
}
