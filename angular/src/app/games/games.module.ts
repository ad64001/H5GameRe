import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CoreModule } from '@abp/ng.core';

import { GamesRoutingModule } from './games-routing.module';
import { GameListComponent } from './game-list/game-list.component';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    GamesRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    GameListComponent
  ]
})
export class GamesModule { }