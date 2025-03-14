import { Component } from '@angular/core';
import { BreadcrumbComponent, BreadcrumbService } from 'xng-breadcrumb';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [
    CommonModule,
    BreadcrumbComponent
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(public breadcrumbService: BreadcrumbService ) { }

}
