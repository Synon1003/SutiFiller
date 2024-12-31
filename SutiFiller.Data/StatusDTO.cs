using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutiFiller.Data
{
    public class StatusDTO
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is StatusDTO dto) && Id == dto.Id;
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
