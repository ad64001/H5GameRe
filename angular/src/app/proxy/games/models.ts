import type { AuditedEntityDto } from '@abp/ng.core';
import type { GameStatus } from './game-status.enum';

export interface GameDto extends AuditedEntityDto<string> {
  name?: string;
  description?: string;
  iconUrl?: string;
  gamePath?: string;
  entryFile?: string;
  status: GameStatus;
  developerName?: string;
  version?: string;
}
