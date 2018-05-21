using System;
using System.Collections.Generic;
using Blackmaw.Dal.Core;

namespace Blackmaw.Dal.Entities
{
    public class Game : EntityBase
    {
        public string Name { get; set; }

        public Guid RuleSystemId { get; set; }
        public virtual RuleSystem RuleSystem { get; set; }
        
        public virtual ICollection<Character> Characters { get; set; } = new HashSet<Character>();
    }
}
