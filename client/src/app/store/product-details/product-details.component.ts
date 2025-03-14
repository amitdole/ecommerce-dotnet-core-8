import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  product?: IProduct;
  quantity = 1;

  constructor(
    private storeService: StoreService, 
    private activatedRoute: ActivatedRoute,
    private breadCrumbService: BreadcrumbService,
    private basketService: BasketService ) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.storeService.getProductById(this.activatedRoute.snapshot.paramMap.get('id')!).subscribe({
      next: response => {
        this.product = response;
        this.breadCrumbService.set('@productDetails', response.name);
        console.log(response);
      },
      error: error => console.log(error),
    });
  }

  
  addItemToCart() {
    if(this.product)
    {
      this.basketService.addItemToBasket(this.product, this.quantity);
    }
    }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if(this.quantity > 1) {
      this.quantity--;
    }
  }

}
