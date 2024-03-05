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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeHistory> EmployeeHistories { get; set; }

    public virtual DbSet<EmployeeProjectAssignment> EmployeeProjectAssignments { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<OrganizationDepartment> OrganizationDepartments { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

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
                .HasConstraintName("employee_ibfk_3");

            entity.HasOne(d => d.Municipality).WithMany(p => p.Employees)
                .HasForeignKey(d => d.MunicipalityId)
                .HasConstraintName("employee_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
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
                .HasConstraintName("employee_history_ibfk_1");

            entity.HasOne(d => d.Position).WithMany(p => p.EmployeeHistories)
                .HasForeignKey(d => d.PositionId)
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
                .HasConstraintName("employee_project_assignment_ibfk_1");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeeProjectAssignments)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("employee_project_assignment_ibfk_2");
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
                .HasConstraintName("project_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}