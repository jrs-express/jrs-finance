using JrsExpressAccounting.Web.Models;
using JrsExpressAccounting.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace JrsExpressAccounting.Web.Data;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options, ICurrentUserService currentUserService) : DbContext(options)
{
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserAccountRole> UserAccountRoles => Set<UserAccountRole>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Party> Parties => Set<Party>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
    public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();
    public DbSet<CheckPrint> CheckPrints => Set<CheckPrint>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccountRole>().HasKey(x => new { x.UserAccountId, x.RoleId });
        modelBuilder.Entity<ChartOfAccount>()
            .HasOne(x => x.ParentAccount)
            .WithMany(x => x.SubAccounts)
            .HasForeignKey(x => x.ParentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntryLine>().Property(x => x.Debit).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<JournalEntryLine>().Property(x => x.Credit).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<JournalEntryLine>().Property(x => x.VatAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<JournalEntryLine>().Property(x => x.WithholdingTaxAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<BankTransaction>().Property(x => x.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<CheckPrint>().Property(x => x.Amount).HasColumnType("decimal(18,2)");

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var username = currentUserService.GetCurrentUsername();
        var changedEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
            .ToList();

        foreach (var entry in changedEntries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAtUtc = DateTime.UtcNow;
                    entity.CreatedBy = username;
                }
                else
                {
                    entity.UpdatedAtUtc = DateTime.UtcNow;
                    entity.UpdatedBy = username;
                }
            }

            AuditLogs.Add(new AuditLog
            {
                Action = entry.State.ToString(),
                TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                RecordId = entry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "new",
                UserName = username,
                ActionTimeUtc = DateTime.UtcNow,
                Changes = string.Join(";", entry.Properties.Where(p => p.IsModified).Select(p => $"{p.Metadata.Name}:{p.OriginalValue}->{p.CurrentValue}"))
            });
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
