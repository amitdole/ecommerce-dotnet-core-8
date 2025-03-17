import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { MsalModule } from '@azure/msal-angular';

@Component({
  selector: 'app-login',
  imports: [CommonModule,
    MsalModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  title = "Login";

  constructor(private acntService: AccountService) { }

  login(){
    this.acntService.login();
  }
}
