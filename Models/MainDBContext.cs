using System;
using System.Collections.Generic;
using KursovaWPF.Resources;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace KursovaWPF.Models;

public partial class MainDBContext : DbContext
{
    public static string Login;
    public static string Password;
    public static string RoleName = "None";
    public MainDBContext()
    {
    }

    public MainDBContext(DbContextOptions<MainDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractView> ContractViews { get; set; }

    public virtual DbSet<Contractee> Contractees { get; set; }

    public virtual DbSet<ContractsDurationView> ContractsDurationViews { get; set; }

    public virtual DbSet<ContractsStatusTable> ContractsStatusTables { get; set; }

    public virtual DbSet<ContractsTimeTable> ContractsTimeTables { get; set; }

    public virtual DbSet<EmpPosition> EmpPositions { get; set; }

    public virtual DbSet<EmpRole> EmpRoles { get; set; }

    public virtual DbSet<EmpTeamMember> EmpTeamMembers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeesSalary> EmployeesSalaries { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ProdDemand> ProdDemands { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProjTask> ProjTasks { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectBacklog> ProjectBacklogs { get; set; }

    public virtual DbSet<ProjectsTimeTable> ProjectsTimeTables { get; set; }
    public virtual DbSet<LogTable> LogTables { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            string connectionString = string.Format("Data Source=DESKTOP-VKFG8N8;Initial Catalog=Проекти IT Гладун;Integrated Security=False;Encrypt=True;Trusted_Connection=False;Trust Server Certificate=True;User Id={0};Password={1};", Login, Password);
            
            optionsBuilder.UseSqlServer(connectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT r.name AS RoleName
                FROM sys.database_role_members m
                INNER JOIN sys.database_principals u ON m.member_principal_id = u.principal_id
                INNER JOIN sys.database_principals r ON m.role_principal_id = r.principal_id
                WHERE u.name = @UserName"
                ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", Login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RoleName = reader["RoleName"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasIndex(e => e.ContractId, "IX_ContractKey");

            entity.HasIndex(e => e.OwnerId, "IX_ContractsOwnerKey");

            entity.Property(e => e.Cost).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne(d => d.Owner).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contracts_Contractees");

            entity.HasOne(d => d.Status).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contracts_ContractsStatusTable");

            entity.HasOne(d => d.TimeTable).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TimeTableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contracts_ContractsTimeTable");
        });

        modelBuilder.Entity<ContractView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ContractView");

            entity.Property(e => e.Contractee)
                .HasMaxLength(60)
                .IsFixedLength();
            entity.Property(e => e.Cost).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(40)
                .IsFixedLength();
        });

        modelBuilder.Entity<Contractee>(entity =>
        {
            entity.HasKey(e => e.ContracteeId).HasName("PK_ProductOwners");

            entity.Property(e => e.ContactInformation)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsFixedLength();
        });

        modelBuilder.Entity<ContractsDurationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ContractsDurationView");
        });

        modelBuilder.Entity<ContractsStatusTable>(entity =>
        {
            entity.HasKey(e => e.ContractStatusId);

            entity.ToTable("ContractsStatusTable");

            entity.Property(e => e.StatusName)
                .HasMaxLength(40)
                .IsFixedLength();
        });

        modelBuilder.Entity<ContractsTimeTable>(entity =>
        {
            entity.HasKey(e => e.TimeTableId).HasName("PK__TimeTabl__BDFA31CDAF086F47");

            entity.ToTable("ContractsTimeTable");
        });

        modelBuilder.Entity<EmpPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId);

            entity.Property(e => e.PayFactor).HasColumnType("numeric(3, 2)");

            entity.HasOne(d => d.Emp).WithMany(p => p.EmpPositions)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpPositions_Employees1");

            entity.HasOne(d => d.Role).WithMany(p => p.EmpPositions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpPositions_EmpRoles");
        });

        modelBuilder.Entity<EmpRole>(entity =>
        {
            entity.HasKey(e => e.EmpRoleId).HasName("PK__EmpRoles__1EBB85AF6950235B");

            entity.Property(e => e.RoleName).HasMaxLength(30);
        });

        modelBuilder.Entity<EmpTeamMember>(entity =>
        {
            entity.HasKey(e => e.TeamMemberId);

            entity.Property(e => e.TeamMemberId).ValueGeneratedNever();

            entity.HasOne(d => d.Emp).WithMany(p => p.EmpTeamMembers)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpTeamMembers_EmpTeamMembers");

            entity.HasOne(d => d.Team).WithMany(p => p.EmpTeamMembers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmpTeamMembers_Teams");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeId, "IX_EmpID");

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "IX_EmpName");

            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Salary).HasColumnType("numeric(10, 3)");
        });

        modelBuilder.Entity<EmployeesSalary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EmployeesSalary");

            entity.Property(e => e.EmployeeKey).ValueGeneratedOnAdd();
            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Pay).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK_CompletePayment");

            entity.ToTable(tb => tb.HasTrigger("CheckPaymentTotal"));

            entity.HasIndex(e => e.ContractId, "IX_PaymentsContractKey");

            entity.Property(e => e.Amount).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Contract).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Contracts");
        });

        modelBuilder.Entity<ProdDemand>(entity =>
        {
            entity.HasKey(e => e.DemandsId).HasName("PK_Demands");

            entity.HasIndex(e => e.ProductId, "IX_ProdDemandsProductKey");

            entity.Property(e => e.Demand).HasColumnType("text");

            entity.HasOne(d => d.Product).WithMany(p => p.ProdDemands)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProdDemands_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsFixedLength();

            entity.HasOne(d => d.Contract).WithMany(p => p.Products)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Contracts");
        });

        modelBuilder.Entity<ProjTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK_Tasks");

            entity.HasIndex(e => e.ProjectId, "IX_ProjTasksProjectKey");

            entity.Property(e => e.Description).HasColumnType("text");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjTasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjTasks_Projects");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.Product).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Products");

            entity.HasOne(d => d.TimeTable).WithMany(p => p.Projects)
                .HasForeignKey(d => d.TimeTableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_ProjectsTimeTable");
        });

        modelBuilder.Entity<ProjectBacklog>(entity =>
        {
            entity.HasKey(e => e.BacklogId).HasName("PK_ProjectBacklog");

            entity.HasIndex(e => e.ProjectId, "IX_ProjectBacklogsProjectKey");

            entity.Property(e => e.ProjectRetrospective).HasColumnType("text");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectBacklogs)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectBacklogs_Projects");
        });

        modelBuilder.Entity<ProjectsTimeTable>(entity =>
        {
            entity.HasKey(e => e.ProjectTimeTableId);

            entity.ToTable("ProjectsTimeTable");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK_Team");

            entity.Property(e => e.TeamName)
                .HasMaxLength(40)
                .IsFixedLength();

            entity.HasOne(d => d.Project).WithMany(p => p.Teams)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_Projects");
        });
        modelBuilder.Entity<LogTable>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("LogTable");

            entity.Property(e => e.LogId).ValueGeneratedNever();
            entity.Property(e => e.Message).IsUnicode(false);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
