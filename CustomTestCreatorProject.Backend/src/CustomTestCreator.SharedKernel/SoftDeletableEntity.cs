namespace CustomTestCreator.SharedKernel;

public abstract class SoftDeletableEntity<TId> : Entity<TId>
{
    public bool IsDeleted { get; private set; }
    public DateTime DeletionDate { get; private set; }
    
    public SoftDeletableEntity(TId id) : base(id)
    {
        
    }

    public virtual void Delete()
    {
        IsDeleted = true;
        DeletionDate = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletionDate = DateTime.UtcNow;
    }
}