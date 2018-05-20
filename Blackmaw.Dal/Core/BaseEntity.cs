using System;
using System.ComponentModel.DataAnnotations.Schema;
using Blackmaw.Dal.Entities;

namespace Blackmaw.Dal.Core
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public Guid CreatedById { get; set; }
        public BmUser CreatedBy { get; set; }

        public Guid ModifiedById { get; set; }
        public BmUser ModifiedBy { get; set; }
    }
}
