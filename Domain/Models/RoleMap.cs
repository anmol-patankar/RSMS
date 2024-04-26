namespace Domain.Models;

public partial class RoleMap
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual UserInfo User { get; set; } = null!;
}