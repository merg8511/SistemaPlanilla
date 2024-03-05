﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SistemaOrbita.Model.Models;

public partial class Employee
{
    public string Id { get; set; }

    public DateTime? EndDate { get; set; }

    public sbyte? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public virtual ICollection<EmployeeHistory> EmployeeHistories { get; set; } = new List<EmployeeHistory>();

    public virtual ICollection<EmployeeProjectAssignment> EmployeeProjectAssignments { get; set; } = new List<EmployeeProjectAssignment>();

    public virtual Gender Gender { get; set; }

    public virtual Municipality Municipality { get; set; }

    public virtual Position Position { get; set; }

}