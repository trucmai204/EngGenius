using EngGenius.Domains.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngGenius.Domains
{
    public class Level
    {
        public EnumLevel Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
