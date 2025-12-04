using Almuerzos.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Almuerzos.Core.Entities
{
   
    public partial class Security 
    {
        public int Id { get; set; } 

        public string Login { get; set; }

        public string Password { get; set; } // Se almacenará el hash

        public string Name { get; set; }

        public RoleType Role { get; set; }
    }
}