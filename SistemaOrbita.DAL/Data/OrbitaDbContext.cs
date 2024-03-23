﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaOrbita.Model.Models;

namespace SistemaOrbita.DAL.Data;

public partial class OrbitaDbContext : DbContext
{
    public OrbitaDbContext(DbContextOptions<OrbitaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeHistory> EmployeeHistories { get; set; }

    public virtual DbSet<EmployeeProjectAssignment> EmployeeProjectAssignments { get; set; }

    public virtual DbSet<Employer> Employers { get; set; }

    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<IncomeTaxBracket> IncomeTaxBrackets { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<OrganizationDepartment> OrganizationDepartments { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<QuotationType> QuotationTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("audit_log");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.ActionType).HasColumnName("action_type");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("datetime")
                .HasColumnName("change_date");
            entity.Property(e => e.Changes).HasColumnName("changes");
            entity.Property(e => e.EntityType).HasColumnName("entity_type");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.GenderId, "gender_id");

            entity.HasIndex(e => e.MunicipalityId, "municipality_id");

            entity.HasIndex(e => e.PositionId, "position_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(230)
                .HasColumnName("address");
            entity.Property(e => e.Birthday)
                .HasColumnType("datetime")
                .HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.Dui)
                .HasMaxLength(9)
                .HasColumnName("dui");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.GenderId)
                .HasMaxLength(45)
                .HasColumnName("gender_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MunicipalityId)
                .HasMaxLength(45)
                .HasColumnName("municipality_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.PositionId)
                .HasMaxLength(45)
                .HasColumnName("position_id");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Gender).WithMany(p => p.Employees)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_ibfk_3");

            entity.HasOne(d => d.Municipality).WithMany(p => p.Employees)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_ibfk_1");
        });

        modelBuilder.Entity<EmployeeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employee_history");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.PositionId, "position_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.ChangeReason)
                .HasMaxLength(255)
                .HasColumnName("change_reason");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(45)
                .HasColumnName("employee_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.NewSalary)
                .HasPrecision(10, 2)
                .HasColumnName("new_salary");
            entity.Property(e => e.OldSalary)
                .HasPrecision(10, 2)
                .HasColumnName("old_salary");
            entity.Property(e => e.PositionId)
                .HasMaxLength(45)
                .HasColumnName("position_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeHistories)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_history_ibfk_1");

            entity.HasOne(d => d.Position).WithMany(p => p.EmployeeHistories)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_history_ibfk_2");
        });

        modelBuilder.Entity<EmployeeProjectAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employee_project_assignment");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.ProjectId, "project_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.AssignedDate)
                .HasColumnType("datetime")
                .HasColumnName("assigned_date");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(45)
                .HasColumnName("employee_id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(45)
                .HasColumnName("project_id");
            entity.Property(e => e.UnassignedDate)
                .HasColumnType("datetime")
                .HasColumnName("unassigned_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeProjectAssignments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_project_assignment_ibfk_1");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeeProjectAssignments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("employee_project_assignment_ibfk_2");
        });

        modelBuilder.Entity<Employer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employer");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.EconomicActivityCode)
                .HasMaxLength(10)
                .HasColumnName("economic_activity_code");
            entity.Property(e => e.LiquidationType).HasColumnName("liquidation_type");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PatronalAfpNumber)
                .HasMaxLength(20)
                .HasColumnName("patronal_afp_number");
            entity.Property(e => e.PatronalInpepNumber)
                .HasMaxLength(20)
                .HasColumnName("patronal_inpep_number");
            entity.Property(e => e.PatronalIsssNumber)
                .HasMaxLength(20)
                .HasColumnName("patronal_isss_number");
            entity.Property(e => e.PayrollType).HasColumnName("payroll_type");
        });

        modelBuilder.Entity<EventLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("event_log");

            entity.HasIndex(e => e.AuthorizedBy, "authorized_by");

            entity.HasIndex(e => e.EmployeeId, "employee_id");

            entity.HasIndex(e => e.EventId, "event_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.AuthorizedBy)
                .HasMaxLength(45)
                .HasColumnName("authorized_by");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(45)
                .HasColumnName("employee_id");
            entity.Property(e => e.EndDte)
                .HasColumnType("datetime")
                .HasColumnName("end_dte");
            entity.Property(e => e.EventId)
                .HasMaxLength(45)
                .HasColumnName("event_id");
            entity.Property(e => e.Frequency).HasColumnName("frequency");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.Recurring).HasColumnName("recurring");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.AuthorizedByNavigation).WithMany(p => p.EventLogAuthorizedByNavigations)
                .HasForeignKey(d => d.AuthorizedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_log_ibfk_3");

            entity.HasOne(d => d.Employee).WithMany(p => p.EventLogEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_log_ibfk_1");

            entity.HasOne(d => d.Event).WithMany(p => p.EventLogs)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_log_ibfk_2");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("event_type");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeduction).HasColumnName("is_deduction");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gender");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<IncomeTaxBracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("income_tax_bracket");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.AboutExcess)
                .HasPrecision(10, 2)
                .HasColumnName("about_excess");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.FixedAmount)
                .HasPrecision(10, 2)
                .HasColumnName("fixed_amount");
            entity.Property(e => e.FromAmount)
                .HasPrecision(10, 2)
                .HasColumnName("from_amount");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Percentage)
                .HasPrecision(10, 2)
                .HasColumnName("percentage");
            entity.Property(e => e.ToAmount)
                .HasPrecision(10, 2)
                .HasColumnName("to_amount");
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("municipality");

            entity.HasIndex(e => e.DepartmentId, "department_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(45)
                .HasColumnName("department_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Department).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("municipality_ibfk_1");
        });

        modelBuilder.Entity<OrganizationDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("organization_department");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("position");

            entity.HasIndex(e => e.OrganizationDepartmentId, "organization_department_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MaxSalary)
                .HasPrecision(10, 2)
                .HasColumnName("max_salary");
            entity.Property(e => e.MinSalary)
                .HasPrecision(10, 2)
                .HasColumnName("min_salary");
            entity.Property(e => e.OrganizationDepartmentId)
                .HasMaxLength(45)
                .HasColumnName("organization_department_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.OrganizationDepartment).WithMany(p => p.Positions)
                .HasForeignKey(d => d.OrganizationDepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("position_ibfk_1");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("project");

            entity.HasIndex(e => e.ManagerId, "manager_id");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.ManagerId)
                .HasMaxLength(45)
                .HasColumnName("manager_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");

            entity.HasOne(d => d.Manager).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("project_ibfk_1");
        });

        modelBuilder.Entity<QuotationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("quotation_type");

            entity.Property(e => e.Id)
                .HasMaxLength(45)
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.EmployeePercentage)
                .HasPrecision(10, 2)
                .HasColumnName("employee_percentage");
            entity.Property(e => e.EmployerPercentage)
                .HasPrecision(10, 2)
                .HasColumnName("employer_percentage");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}