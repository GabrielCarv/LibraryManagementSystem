namespace Library_Management_System.Entity
{
    public interface IEntityBase
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
