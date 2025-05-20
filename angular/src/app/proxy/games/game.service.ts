import type { CreateGameDto, GetGameListDto, UpdateGameStatusDto } from './dtos/models';
import type { GameDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  apiName = 'Default';
  

  changeStatus = (id: string, input: UpdateGameStatusDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GameDto>({
      method: 'POST',
      url: `/api/app/game/${id}/change-status`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateGameDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GameDto>({
      method: 'POST',
      url: '/api/app/game',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/game/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GameDto>({
      method: 'GET',
      url: `/api/app/game/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getGameEntryUrl = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: `/api/app/game/${id}/game-entry-url`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetGameListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GameDto>>({
      method: 'GET',
      url: '/api/app/game',
      params: { filter: input.filter, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateGameDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GameDto>({
      method: 'PUT',
      url: `/api/app/game/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
