namespace Domain.Models;

public partial class UserInfo
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] PasswordHashed { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? StoreId { get; set; }

    public DateOnly Dob { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual ICollection<PayrollHistory> PayrollHistoryAuthorizers { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<PayrollHistory> PayrollHistoryPayees { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<RoleMap> RoleMaps { get; set; } = new List<RoleMap>();

    public virtual Store? Store { get; set; }

    public virtual ICollection<Transaction> TransactionCashiers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionCustomers { get; set; } = new List<Transaction>();
}