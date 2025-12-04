using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Almuerzos.Core.Enum;

namespace Almuerzos.Infrastructure.DTOs
{
    public class SecurityDto
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public RoleType? Role { get; set; }
    }
}
