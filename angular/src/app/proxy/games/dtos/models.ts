import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { GameStatus } from '../game-status.enum';

export interface CreateGameDto {
  name: string;
  description?: string;
  iconUrl?: string;
  gamePath?: string;
  entryFile?: string;
  developerName?: string;
  version?: string;
}

export interface GetGameListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  status?: GameStatus;
}

export interface UpdateGameStatusDto {
  status: GameStatus;
}
