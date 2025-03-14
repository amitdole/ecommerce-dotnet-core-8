import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/Type';
import { StoreParams } from '../shared/models/storeParams';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  constructor(private httpClient: HttpClient) { }

  baseUrl = 'http://localhost:8010/';

  getProductById(id: string){
    return this.httpClient.get<IProduct>(this.baseUrl + 'Catalog/GetProductById/' + id);
  }

  getProducts(storeParams: StoreParams){
    let params = new HttpParams();
    if(storeParams.brandId){
      params = params.append('brandId', storeParams.brandId.toString());
    }
    if(storeParams.typeId){
      params = params.append('typeId', storeParams.typeId.toString());
    }

    if(storeParams.search){
      params = params.append('search', storeParams.search);
    }

    params = params.append('sort', storeParams.sort);
    

    return this.httpClient.get<IPagination<IProduct[]>>(this.baseUrl + 'Catalog/GetAllProducts', {params});
  }

  getBrands(){
    return this.httpClient.get<IBrand[]>(this.baseUrl + 'Catalog/GetAllBrands');
  }

  getTypes(){
    return this.httpClient.get<IType[]>(this.baseUrl + 'Catalog/GetAllTypes');
  }
}
