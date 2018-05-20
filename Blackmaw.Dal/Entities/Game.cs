using System;
using System.Collections.Generic;
using Blackmaw.Dal.Core;

namespace Blackmaw.Dal.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; set; }

        public Guid RuleSystemId { get; set; }
        public RuleSystem RuleSystem { get; set; }
        
        public ICollection<Character> Characters { get; set; } = new HashSet<Character>();
    }
}
