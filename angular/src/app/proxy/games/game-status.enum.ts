import { mapEnumToOptions } from '@abp/ng.core';

export enum GameStatus {
  PendingReview = 0,
  InReview = 1,
  Approved = 2,
  Rejected = 3,
}

export const gameStatusOptions = mapEnumToOptions(GameStatus);
