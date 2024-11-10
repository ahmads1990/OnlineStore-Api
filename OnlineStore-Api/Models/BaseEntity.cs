namespace OnlineStore_Api.Models;

public class BaseEntity
{
    public int ID { get; set; }
    public bool Deleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
