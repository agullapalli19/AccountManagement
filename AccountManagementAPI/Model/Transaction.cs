using System;
using System.Collections.Generic;

namespace AccountManagementAPI.Model;

public partial class Transaction
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int SourceAccountId { get; set; }

    public int TargetAccountId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? Description { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Account SourceAccount { get; set; } = null!;

    public virtual Account TargetAccount { get; set; } = null!;
}
