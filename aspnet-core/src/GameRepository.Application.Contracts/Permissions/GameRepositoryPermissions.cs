namespace GameRepository.Permissions;

public static class GameRepositoryPermissions
{
    public const string GroupName = "GameRepository";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";
    }

    // 添加Games权限组
    public static class Games
    {
        public const string Default = GroupName + ".Games";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Manage = Default + ".Manage"; // 用于审核游戏
    }
}
