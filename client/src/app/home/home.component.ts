import { Component } from '@angular/core';
import { CarouselComponent, SlideComponent } from 'ngx-bootstrap/carousel';

@Component({
  selector: 'app-home',
  imports: [CarouselComponent,SlideComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {

}
