import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../_services/account.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from './../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService:AccountService,private toastr:ToastrService){}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        console.log(user)
        if(user)return true; 
        this.toastr.error('you shall not pass!')
        return false;
      })
    );
  }
  
}
