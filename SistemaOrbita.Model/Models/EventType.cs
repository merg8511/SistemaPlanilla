﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SistemaOrbita.Model.Models;

public partial class EventType
{
    public string Id { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public sbyte? IsActive { get; set; }

    public virtual ICollection<EventLog> EventLogs { get; set; } = new List<EventLog>();
}