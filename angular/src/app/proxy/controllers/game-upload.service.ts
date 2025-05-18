import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GameDto } from '../games/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class GameUploadService {
  apiName = 'Default';
  

  uploadGame = (name: string, description: string, developerName: string, version: string, icon: IFormFile, gamePackage: IFormFile, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GameDto>({
      method: 'POST',
      url: '/api/game-repository/games/upload',
      params: { name, description, developerName, version },
      body: gamePackage,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
