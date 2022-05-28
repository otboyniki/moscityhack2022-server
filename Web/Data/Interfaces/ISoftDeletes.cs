namespace Web.Data.Interfaces;

public interface ISoftDeletes
{
    DateTime? DeletedAt { get; set; }
}