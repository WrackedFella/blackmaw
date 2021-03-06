﻿using Blackmaw.Dal.Core;
using System;

namespace Blackmaw.Dal.Entities
{
    public class Character : EntityBase
    {
        public string Name { get; set; }

        public Guid RuleSystemId { get; set; }
        public virtual RuleSystem RuleSystem { get; set; }
    }
}
