using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library_Management_System.Models;

namespace Library_Management_System.Maps
{
    public class BookMap : EntityMapBase<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);
        }
    }
}
