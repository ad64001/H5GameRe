using GameRepository.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace GameRepository.Permissions;

public class GameRepositoryPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GameRepositoryPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(GameRepositoryPermissions.MyPermission1, L("Permission:MyPermission1"));

        // 添加Games权限定义
        var gamesPermission = myGroup.AddPermission(
            GameRepositoryPermissions.Games.Default,
            L("Permission:Games"));

        gamesPermission.AddChild(
            GameRepositoryPermissions.Games.Create,
            L("Permission:Games.Create"));

        gamesPermission.AddChild(
            GameRepositoryPermissions.Games.Edit,
            L("Permission:Games.Edit"));

        gamesPermission.AddChild(
            GameRepositoryPermissions.Games.Delete,
            L("Permission:Games.Delete"));

        gamesPermission.AddChild(
            GameRepositoryPermissions.Games.Manage,
            L("Permission:Games.Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GameRepositoryResource>(name);
    }
}
