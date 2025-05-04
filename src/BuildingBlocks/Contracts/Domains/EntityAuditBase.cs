using Contracts.Domains.Interfaces;

namespace Contracts.Domains
{
    internal class EntityAuditBase<Tkey> : EntityBase<Tkey>, IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
