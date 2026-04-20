using JrsExpressAccounting.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace JrsExpressAccounting.Web.Migrations
{
    [DbContext(typeof(AccountingDbContext))]
    partial class AccountingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");
        }
    }
}
