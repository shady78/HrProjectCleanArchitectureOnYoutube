namespace HRManagement.Domain.Entities
{
    public static class SystemPermissions
    {
        public static class Modules
        {
            public const string Employees = "Employees";
            public const string Departments = "Departments";
            public const string JobTitles = "JobTitles";
            public const string Roles = "Roles";
            public const string Users = "Users";
            public const string AuditLogs = "AuditLogs";
            public const string DeveloperLogs = "DeveloperLogs";
        }
        public static class Employees
        {
            public const string View = "Employees.View";
            public const string Create = "Employees.Create";
            public const string Update = "Employees.Update";
            public const string Delete = "Employees.Delete";
        }

        // Department Permissions
        public static class Departments
        {
            public const string View = "Departments.View";
            public const string Create = "Departments.Create";
            public const string Update = "Departments.Update";
            public const string Delete = "Departments.Delete";
        }

        // Roles Permission
        public static class Roles
        {
            public const string RolesView = "Roles.View";
            public const string RolesCreate = "Roles.Create";
            public const string RolesUpdate = "Roles.Update";
            public const string RolesDelete = "Roles.Delete";
            public const string RolesManagePermissions = "Roles.ManagePermissions";
        }
        public static class AuditLogs
        {
            public const string View = "AuditLogs.View";
        }
        public static class Users
        {
            public const string View = "Users.View";
            public const string Update = "Users.Update";
            public const string Block = "Users.Block";
            public const string Unblock = "Users.Unblock";
            public const string ViewPermissions = "Users.ViewPermissions";
            public const string ManagePermissions = "Users.ManagePermissions";
        }
        public static class DeveloperLogs
        {
            public const string View = "DeveloperLogs.View";
        }
        public static readonly PermissionDefinition[] All =
     [
        new(Employees.View, "Can view employees",  Modules.Employees),
        new(Employees.Create, "Can create employees",  Modules.Employees),
        new(Employees.Update, "Can update employees",  Modules.Employees),
        new(Employees.Delete, "Can delete employees",  Modules.Employees),

        new(Departments.View, "Can view departments",  Modules.Departments),
        new(Departments.Create, "Can create departments",  Modules.Departments),
        new(Departments.Update, "Can update departments",  Modules.Departments),
        new(Departments.Delete, "Can delete departments",  Modules.Departments),

        //new(JobTitles.View, "Can view job titles",  Modules.JobTitles),
        //new(JobTitles.Create, "Can create job titles",  Modules.JobTitles),
        //new(JobTitles.Update, "Can update job titles",  Modules.JobTitles),
        //new(JobTitles.Delete, "Can delete job titles",  Modules.JobTitles),

        new(Roles.RolesView, "Can view roles",  Modules.Roles),
        new(Roles.RolesCreate, "Can create roles",  Modules.Roles),
        new(Roles.RolesUpdate, "Can update roles",  Modules.Roles),
        new(Roles.RolesDelete, "Can delete roles",  Modules.Roles),
        new(Roles.RolesManagePermissions, "Can manage role permissions",  Modules.Roles),

        new(Users.View, "Can view users",  Modules.Users),
        new(Users.Update, "Can update users",  Modules.Users),
        new(Users.Block, "Can block users",  Modules.Users),
        new(Users.Unblock, "Can unblock users",  Modules.Users),
        new(Users.ViewPermissions, "Can view direct user permissions", Modules.Users),
        new(Users.ManagePermissions, "Can manage direct user permissions", Modules.Users),

        new(AuditLogs.View, "Can view audit logs",  Modules.AuditLogs),
        new(DeveloperLogs.View, "Can view developer logs", Modules.DeveloperLogs)
     ];
    }
}
