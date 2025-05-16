import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'GameRepository',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44390/',
    redirectUri: baseUrl,
    clientId: 'GameRepository_App',
    responseType: 'code',
    scope: 'offline_access GameRepository',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44390',
      rootNamespace: 'GameRepository',
    },
  },
} as Environment;
