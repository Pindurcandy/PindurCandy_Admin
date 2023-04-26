using System;
using System.Collections.Generic;

namespace PindurCandy_Admin.Models;

public partial class Termekek
{
    public int Id { get; set; }

    public string TermekNev { get; set; } = null!;

    public byte[] Kep { get; set; } = null!;

    public int Ar { get; set; }

    public string Leiras { get; set; } = null!;

    public int Aktiv { get; set; }

    public string Link { get; set; } = null!;
}
