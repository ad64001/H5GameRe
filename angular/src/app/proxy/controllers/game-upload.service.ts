import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GameDto } from '../games/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class GameUploadService {
  apiName = 'Default';

  updateGame = (id: string, name: string, description: string, developerName: string, version: string, icon: File | null, gamePackage: File | null, config?: Partial<Rest.Config>) => {
    const formData = new FormData();

    // 只有当文件存在时才添加到FormData
    if (icon) {
      formData.append('icon', icon);
    }

    if (gamePackage) {
      formData.append('gamePackage', gamePackage);
    }

    return this.restService.request<any, GameDto>({
      method: 'PUT',
      url: '/api/game-repository/games/upload',
      params: { id, name, description, developerName, version },
      body: formData,
    }, { apiName: this.apiName, ...config });
  }

  uploadGame = (name: string, description: string, developerName: string, version: string, icon: File, gamePackage: File, config?: Partial<Rest.Config>) => {
    const formData = new FormData();
    formData.append('icon', icon);
    formData.append('gamePackage', gamePackage);

    return this.restService.request<any, GameDto>({
      method: 'POST',
      url: '/api/game-repository/games/upload',
      params: { name, description, developerName, version },
      body: formData,
    }, { apiName: this.apiName, ...config });
  }

  constructor(private restService: RestService) {}
}
