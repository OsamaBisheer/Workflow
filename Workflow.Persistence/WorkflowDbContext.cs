using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml.Linq;
using Workflow.Domain.Entities;
using Workflow.Domain.Entities.Identity;
using Workflow.Domain.Interfaces.ICore;
using static Workflow.Domain.Enums.Enumeration;

namespace Workflow.Persistence
{
    public class WorkflowDbContext : IdentityDbContext<User, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IWorkflowDbContext
    {
        public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options)
        { }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationRoleDescription> ApplicationRoleDescriptions { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Workflow.Domain.Entities.Workflow> Workflows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Process>(p =>
            {
                p.HasOne(pr => pr.CurrentInitiator)
                    .WithMany(u => u.InitatorProcesses)
                    .HasForeignKey(pr => pr.CurrentInitiatorId);

                p.HasOne(pr => pr.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(pr => pr.CreatedByUserId);
            });

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "1",
                    Name = "1",
                    NormalizedName = "1"
                },
                new ApplicationRole
                {
                    Id = "2",
                    Name = "2",
                    NormalizedName = "2"
                },
                new ApplicationRole
                {
                    Id = "3",
                    Name = "3",
                    NormalizedName = "3"
                },
                new ApplicationRole
                {
                    Id = "4",
                    Name = "4",
                    NormalizedName = "4"
                }
        );

            builder.Entity<ApplicationRoleDescription>().HasData(
                new ApplicationRoleDescription
                {
                    Id = "1",
                    ApplicationRoleDescriptionEnum = ApplicationRoleDescriptionEnum.Manager,
                    NameFL = nameof(ApplicationRoleDescriptionEnum.Manager),
                    NameSL = nameof(ApplicationRoleDescriptionEnum.Manager)
                },
                new ApplicationRoleDescription
                {
                    Id = "2",
                    ApplicationRoleDescriptionEnum = ApplicationRoleDescriptionEnum.Finance,
                    NameFL = nameof(ApplicationRoleDescriptionEnum.Finance),
                    NameSL = nameof(ApplicationRoleDescriptionEnum.Finance)
                },
                new ApplicationRoleDescription
                {
                    Id = "3",
                    ApplicationRoleDescriptionEnum = ApplicationRoleDescriptionEnum.Employee,
                    NameFL = nameof(ApplicationRoleDescriptionEnum.Employee),
                    NameSL = nameof(ApplicationRoleDescriptionEnum.Employee)
                },
                new ApplicationRoleDescription
                {
                    Id = "4",
                    ApplicationRoleDescriptionEnum = ApplicationRoleDescriptionEnum.System,
                    NameFL = nameof(ApplicationRoleDescriptionEnum.System),
                    NameSL = nameof(ApplicationRoleDescriptionEnum.System)
                }
        );

            builder.Entity<ActionType>().HasData(
                    new ActionType
                    {
                        Id = 1,
                        ActionTypeEnum = ActionTypeEnum.Initial,
                        NameFL = nameof(ActionTypeEnum.Initial),
                        NameSL = nameof(ActionTypeEnum.Initial)
                    },
                     new ActionType
                     {
                         Id = 2,
                         ActionTypeEnum = ActionTypeEnum.Input,
                         NameFL = nameof(ActionTypeEnum.Input),
                         NameSL = nameof(ActionTypeEnum.Input)
                     },
                     new ActionType
                     {
                         Id = 3,
                         ActionTypeEnum = ActionTypeEnum.Output,
                         NameFL = nameof(ActionTypeEnum.Output),
                         NameSL = nameof(ActionTypeEnum.Output)
                     },
                     new ActionType
                     {
                         Id = 4,
                         ActionTypeEnum = ActionTypeEnum.ApprovalReject,
                         NameFL = nameof(ActionTypeEnum.ApprovalReject),
                         NameSL = nameof(ActionTypeEnum.ApprovalReject)
                     },
                      new ActionType
                      {
                          Id = 5,
                          ActionTypeEnum = ActionTypeEnum.End,
                          NameFL = nameof(ActionTypeEnum.End),
                          NameSL = nameof(ActionTypeEnum.End)
                      }
                            );
        }
    }
}