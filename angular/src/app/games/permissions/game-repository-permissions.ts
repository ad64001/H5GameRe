export namespace GameRepositoryPermissions {
  export const GroupName = 'GameRepository';

  export class Games {
    static readonly Default = 'GameRepository.Games';
    static readonly Create = 'GameRepository.Games.Create';
    static readonly Edit = 'GameRepository.Games.Edit';
    static readonly Delete = 'GameRepository.Games.Delete';
    static readonly Manage = 'GameRepository.Games.Manage';
  }
}