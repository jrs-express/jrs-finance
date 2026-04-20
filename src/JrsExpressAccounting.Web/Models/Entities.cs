using System.ComponentModel.DataAnnotations;

namespace JrsExpressAccounting.Web.Models;

public enum PartyType { Customer, Vendor }
public enum BankTxnType { Deposit, Withdrawal }

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = "system";
    public DateTime? UpdatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }
}

public class UserAccount : BaseEntity
{
    [MaxLength(80)] public string Username { get; set; } = string.Empty;
    [MaxLength(256)] public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(80)] public string FullName { get; set; } = string.Empty;
    [MaxLength(80)] public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<UserAccountRole> Roles { get; set; } = new List<UserAccountRole>();
}

public class Role : BaseEntity
{
    [MaxLength(50)] public string Name { get; set; } = string.Empty;
    public ICollection<UserAccountRole> Users { get; set; } = new List<UserAccountRole>();
}

public class UserAccountRole
{
    public int UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = default!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}

public class Branch : BaseEntity
{
    [MaxLength(20)] public string Code { get; set; } = string.Empty;
    [MaxLength(150)] public string Name { get; set; } = string.Empty;
    [MaxLength(500)] public string Address { get; set; } = string.Empty;
    [MaxLength(20)] public string Tin { get; set; } = string.Empty;
}

public class ChartOfAccount : BaseEntity
{
    [MaxLength(20)] public string Code { get; set; } = string.Empty;
    [MaxLength(150)] public string Name { get; set; } = string.Empty;
    [MaxLength(50)] public string AccountType { get; set; } = string.Empty;
    public int? ParentAccountId { get; set; }
    public ChartOfAccount? ParentAccount { get; set; }
    public ICollection<ChartOfAccount> SubAccounts { get; set; } = new List<ChartOfAccount>();
}

public class Bank : BaseEntity
{
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(30)] public string AccountNumber { get; set; } = string.Empty;
    public int ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; } = default!;
}

public class Party : BaseEntity
{
    [MaxLength(150)] public string Name { get; set; } = string.Empty;
    [MaxLength(20)] public string Tin { get; set; } = string.Empty;
    [MaxLength(500)] public string Address { get; set; } = string.Empty;
    [MaxLength(100)] public string BusinessStyle { get; set; } = string.Empty;
    [MaxLength(50)] public string TaxType { get; set; } = "VAT";
    [MaxLength(50)] public string ContactNumber { get; set; } = string.Empty;
    [MaxLength(100)] public string Email { get; set; } = string.Empty;
    public PartyType PartyType { get; set; }
}

public class JournalEntry : BaseEntity
{
    [MaxLength(30)] public string EntryNo { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    [MaxLength(500)] public string Particulars { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = default!;
    public ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
}

public class JournalEntryLine
{
    public int Id { get; set; }
    public int JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = default!;
    public int ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; } = default!;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    [MaxLength(200)] public string Description { get; set; } = string.Empty;
    public decimal VatAmount { get; set; }
    public decimal WithholdingTaxAmount { get; set; }
}

public class BankTransaction : BaseEntity
{
    public DateTime TxnDate { get; set; } = DateTime.UtcNow;
    public BankTxnType TxnType { get; set; }
    public decimal Amount { get; set; }
    [MaxLength(250)] public string Remarks { get; set; } = string.Empty;
    public int BankId { get; set; }
    public Bank Bank { get; set; } = default!;
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = default!;
}

public class CheckPrint : BaseEntity
{
    [MaxLength(30)] public string CheckNo { get; set; } = string.Empty;
    public DateTime CheckDate { get; set; }
    [MaxLength(150)] public string Payee { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    [MaxLength(300)] public string Purpose { get; set; } = string.Empty;
    public bool IsPrinted { get; set; }
}

public class AuditLog
{
    public int Id { get; set; }
    [MaxLength(50)] public string Action { get; set; } = string.Empty;
    [MaxLength(120)] public string TableName { get; set; } = string.Empty;
    [MaxLength(50)] public string RecordId { get; set; } = string.Empty;
    [MaxLength(100)] public string UserName { get; set; } = string.Empty;
    public DateTime ActionTimeUtc { get; set; } = DateTime.UtcNow;
    [MaxLength(2000)] public string? Changes { get; set; }
}
