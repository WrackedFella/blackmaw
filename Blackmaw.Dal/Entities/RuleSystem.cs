﻿using System.Collections.Generic;
using Blackmaw.Dal.Core;

namespace Blackmaw.Dal.Entities
{
    public class RuleSystem : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; } = new HashSet<Game>();
    }
}
