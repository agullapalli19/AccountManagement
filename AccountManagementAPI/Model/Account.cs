using System;
using System.Collections.Generic;

namespace AccountManagementAPI.Model;

public partial class Account
{
    public int AccountId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public int? AccountHolderId { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Person? AccountHolder { get; set; }

    public virtual ICollection<Transaction> TransactionSourceAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionTargetAccounts { get; set; } = new List<Transaction>();
}
