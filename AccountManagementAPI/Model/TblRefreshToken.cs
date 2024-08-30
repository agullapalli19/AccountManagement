using System;
using System.Collections.Generic;

namespace AccountManagementAPI.Model;

public partial class TblRefreshToken
{
    public string? UserId { get; set; }

    public string? TokenId { get; set; }

    public string? RefreshToken { get; set; }

    public int? IsActive { get; set; }
}
