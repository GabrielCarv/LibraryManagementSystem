using Library_Management_System.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Library_Management_System.Maps
{
    public abstract class EntityMapBase<TEntity> : IEntityMap<TEntity> where TEntity : class, IEntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(t => t.IsDeleted == false);
        }
    }
}
