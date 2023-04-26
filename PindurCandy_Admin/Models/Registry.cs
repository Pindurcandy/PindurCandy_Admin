using System;
using System.Collections.Generic;

namespace PindurCandy_Admin.Models;

public partial class Registry
{
    public int Id { get; set; }

    public string FelhasznaloNev { get; set; } = null!;

    public string TeljesNev { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Key { get; set; } = null!;
}
