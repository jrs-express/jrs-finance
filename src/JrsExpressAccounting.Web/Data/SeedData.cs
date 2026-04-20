using JrsExpressAccounting.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace JrsExpressAccounting.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AccountingDbContext db)
    {
        if (await db.Roles.AnyAsync()) return;

        var adminRole = new Role { Name = "Admin" };
        var accountingAdminRole = new Role { Name = "AccountingAdmin" };
        var accountingRole = new Role { Name = "Accounting" };

        db.Roles.AddRange(adminRole, accountingAdminRole, accountingRole);

        var adminUser = new UserAccount
        {
            Username = "admin",
            FullName = "System Admin",
            Email = "admin@jrsexpress.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
        };
        var acctUser = new UserAccount
        {
            Username = "accounting",
            FullName = "Accounting User",
            Email = "accounting@jrsexpress.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Accounting@123")
        };

        db.UserAccounts.AddRange(adminUser, acctUser);
        await db.SaveChangesAsync();

        db.UserAccountRoles.AddRange(
            new UserAccountRole { UserAccountId = adminUser.Id, RoleId = adminRole.Id },
            new UserAccountRole { UserAccountId = adminUser.Id, RoleId = accountingAdminRole.Id },
            new UserAccountRole { UserAccountId = acctUser.Id, RoleId = accountingRole.Id });

        var mainBranch = new Branch { Code = "MNL", Name = "JRS Main Manila", Address = "Manila", Tin = "123-456-789-000" };
        db.Branches.Add(mainBranch);

        var cash = new ChartOfAccount { Code = "1010", Name = "Cash on Hand", AccountType = "Asset" };
        var bank = new ChartOfAccount { Code = "1020", Name = "Cash in Bank", AccountType = "Asset" };
        var revenue = new ChartOfAccount { Code = "4010", Name = "Service Revenue", AccountType = "Income" };
        var expense = new ChartOfAccount { Code = "5010", Name = "Delivery Expense", AccountType = "Expense" };
        db.ChartOfAccounts.AddRange(cash, bank, revenue, expense);

        await db.SaveChangesAsync();

        db.Banks.Add(new Bank { Name = "BDO", AccountNumber = "00123456789", ChartOfAccountId = bank.Id });

        db.Parties.AddRange(
            new Party { Name = "ABC Trading", PartyType = PartyType.Customer, Tin = "111-222-333-000", Address = "Quezon City", BusinessStyle = "Trading", TaxType = "VAT", ContactNumber = "09170000001", Email = "abc@customer.com" },
            new Party { Name = "XYZ Supplies", PartyType = PartyType.Vendor, Tin = "222-333-444-000", Address = "Makati", BusinessStyle = "Supplies", TaxType = "Non-VAT", ContactNumber = "09170000002", Email = "xyz@vendor.com" }
        );

        await db.SaveChangesAsync();
    }
}
