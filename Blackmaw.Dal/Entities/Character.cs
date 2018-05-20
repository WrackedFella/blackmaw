using Blackmaw.Dal.Core;
using System;

namespace Blackmaw.Dal.Entities
{
    public class Character : BaseEntity
    {
        public string Name { get; set; }

        public Guid RuleSystemId { get; set; }
        public RuleSystem RuleSystem { get; set; }
    }
}
