﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SistemaOrbita.Model.Models;

public partial class Employer
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string EconomicActivityCode { get; set; }

    public string PatronalIsssNumber { get; set; }

    public string PatronalAfpNumber { get; set; }

    public string PatronalInpepNumber { get; set; }

    public sbyte? LiquidationType { get; set; }

    public sbyte? PayrollType { get; set; }
}