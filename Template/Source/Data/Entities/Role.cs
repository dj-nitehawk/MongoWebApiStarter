using MongoDB.Entities;
using MongoDB.Entities.Core;
using System.Collections.Generic;

namespace Data
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public bool SystemRole { get; set; }
        public IEnumerable<ushort> Permissions { get; set; }
        public One<Account> Account { get; set; }
    }
}
