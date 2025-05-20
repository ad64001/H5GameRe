import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GameDto } from '../games/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class GameUploadService {
  apiName = 'Default';

  uploadGame = (name: string, description: string, developerName: string, version: string, icon: IFormFile, gamePackage: IFormFile, config?: Partial<Rest.Config>) => {
    const formData = new FormData();
    formData.append('name', name);
    formData.append('description', description);
    formData.append('developerName', developerName);
    formData.append('version', version);
    formData.append('icon', icon as any);
    formData.append('gamePackage', gamePackage as any);

    return this.restService.request<any, GameDto>({
      method: 'POST',
      url: '/api/game-repository/games/upload',
      body: formData,
    },
    { apiName: this.apiName, ...config });
  }

  constructor(private restService: RestService) {}
}