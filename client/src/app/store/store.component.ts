import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { IProduct } from '../shared/models/product';
import { CommonModule } from '@angular/common';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/Type';
import { StoreParams } from '../shared/models/storeParams';
import { RouterLink } from '@angular/router';
import { BasketService } from '../basket/basket.service';


@Component({
  selector: 'app-store',
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './store.component.html',
  styleUrl: './store.component.scss',
})
export class StoreComponent implements OnInit {
  @ViewChild('search') searchTerm: ElementRef | undefined;
  products:IProduct[] = [];
  brands:IBrand[] = [];
  types:IType[] = [];
  storeParams = new StoreParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Ascending', value: 'priceAsc' },
    { name: 'Price: Descending', value: 'priceDesc'}
  ];

  constructor(private storeService: StoreService, private basketService: BasketService) { }

  ngOnInit(): void {
  
    this.getProducts();
    this.getBrands();
    this.getTypes();

  }

  getProducts() {
    this.storeService.getProducts(this.storeParams).subscribe({
      next: response => {
        this.products = response.data;
        console.log(response);
      },
      error: error => console.log(error),
    });
  }

  getBrands() {
    this.storeService.getBrands().subscribe({
      next: response => {
        this.brands = [{id:'', name:'All'}, ...response];
        console.log(response);
      },
      error: error => console.log(error),
    });
  }

  getTypes() {
    this.storeService.getTypes().subscribe({
      next: response => {
        this.types = [{id:'', name:'All'}, ...response];
        console.log(response);
      },
      error: error => console.log(error),
    });
  }

  onBrandSelected(brandId: string) {
    this.storeParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(typeId: string) {
    this.storeParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onSearch() {
    this.storeParams.search = this.searchTerm?.nativeElement.value;
    this.storeParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    if(this.searchTerm){
      if (this.searchTerm) {
        this.searchTerm.nativeElement.value = '';
      }
      this.storeParams = new StoreParams();
      this.getProducts();
    }
  }

  addItemToBasket(productId: IProduct) { 
    this.basketService.addItemToBasket(productId);
  }
    
}
