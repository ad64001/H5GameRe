# GameRepository

This is a startup project based on the ABP framework. For more information, visit <a href="https://abp.io/" target="_blank">abp.io</a>

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.


```
angular
├─ 📁.angular
├─ 📁src
│  ├─ 📁app
│  │  ├─ 📁games
│  │  │  ├─ 📁game-list
│  │  │  │  ├─ 📄game-list.component.html
│  │  │  │  ├─ 📄game-list.component.scss
│  │  │  │  ├─ 📄game-list.component.spec.ts
│  │  │  │  └─ 📄game-list.component.ts
│  │  │  ├─ 📄games-routing.module.ts
│  │  │  └─ 📄games.module.ts
│  │  ├─ 📁home
│  │  │  ├─ 📄home-routing.module.ts
│  │  │  ├─ 📄home.component.html
│  │  │  ├─ 📄home.component.scss
│  │  │  ├─ 📄home.component.spec.ts
│  │  │  ├─ 📄home.component.ts
│  │  │  └─ 📄home.module.ts
│  │  ├─ 📁proxy
│  │  │  ├─ 📁controllers
│  │  │  │  ├─ 📄game-upload.service.ts
│  │  │  │  └─ 📄index.ts
│  │  │  ├─ 📁games
│  │  │  │  ├─ 📁dtos
│  │  │  │  │  ├─ 📄index.ts
│  │  │  │  │  └─ 📄models.ts
│  │  │  │  ├─ 📄game-status.enum.ts
│  │  │  │  ├─ 📄game.service.ts
│  │  │  │  ├─ 📄index.ts
│  │  │  │  └─ 📄models.ts
│  │  │  ├─ 📁microsoft
│  │  │  │  ├─ 📁asp-net-core
│  │  │  │  │  ├─ 📁http
│  │  │  │  │  │  ├─ 📄index.ts
│  │  │  │  │  │  └─ 📄models.ts
│  │  │  │  │  └─ 📄index.ts
│  │  │  │  └─ 📄index.ts
│  │  │  ├─ 📄generate-proxy.json
│  │  │  ├─ 📄index.ts
│  │  │  └─ 📄README.md
│  │  ├─ 📁shared
│  │  │  └─ 📄shared.module.ts
│  │  ├─ 📄app-routing.module.ts
│  │  ├─ 📄app.component.ts
│  │  ├─ 📄app.module.ts
│  │  └─ 📄route.provider.ts
│  ├─ 📁assets
│  │  ├─ 📁images
│  │  │  ├─ 📁getting-started
│  │  │  │  ├─ 📄abp-blog.svg
│  │  │  │  ├─ 📄abp-community.svg
│  │  │  │  ├─ 📄abp-support.svg
│  │  │  │  ├─ 📄bg-01.png
│  │  │  │  ├─ 📄book.png
│  │  │  │  ├─ 📄discord.svg
│  │  │  │  ├─ 📄img-blog.png
│  │  │  │  ├─ 📄img-community.png
│  │  │  │  ├─ 📄img-support.png
│  │  │  │  ├─ 📄instagram.svg
│  │  │  │  ├─ 📄stack-overflow.svg
│  │  │  │  ├─ 📄x.svg
│  │  │  │  └─ 📄youtube.svg
│  │  │  └─ 📁logo
│  │  │     ├─ 📄logo-light-thumbnail.png
│  │  │     └─ 📄logo-light.png
│  │  └─ 📄.gitkeep
│  ├─ 📁environments
│  │  ├─ 📄environment.prod.ts
│  │  └─ 📄environment.ts
│  ├─ 📄favicon.ico
│  ├─ 📄index.html
│  ├─ 📄main.ts
│  ├─ 📄polyfills.ts
│  ├─ 📄styles.scss
│  └─ 📄test.ts
├─ 📄.editorconfig
├─ 📄.eslintrc.json
├─ 📄.gitignore
├─ 📄.prettierrc
├─ 📄angular.json
├─ 📄karma.conf.js
├─ 📄package.json
├─ 📄README.md
├─ 📄start.ps1
├─ 📄tsconfig.app.json
├─ 📄tsconfig.json
├─ 📄tsconfig.spec.json
└─ 📄yarn.lock
```