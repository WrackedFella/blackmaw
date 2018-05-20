using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Blackmaw.Dal.Entities
{
    public class BmUser : IdentityUser<Guid>
    {
        public ICollection<Game> Games { get; set; } = new HashSet<Game>();
        public ICollection<RuleSystem> RuleSystems { get; set; } = new HashSet<RuleSystem>();
    }
}
