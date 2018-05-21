using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Blackmaw.Dal.Entities
{
    public class BlackmawUser : IdentityUser<Guid>
    {
        public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
        public virtual ICollection<RuleSystem> RuleSystems { get; set; } = new HashSet<RuleSystem>();
    }
}
