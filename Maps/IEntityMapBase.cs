using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Maps
{
    public interface IEntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {

    }
}
