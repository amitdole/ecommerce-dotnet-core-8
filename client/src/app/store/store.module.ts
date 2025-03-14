import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreComponent } from './store.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { RouterLink } from '@angular/router';


@NgModule({
  declarations: [],
  imports: [
    StoreComponent,
    CommonModule,
    RouterLink
  ],
  exports: [
    StoreComponent,
  ]
})
export class StoreModule { }
