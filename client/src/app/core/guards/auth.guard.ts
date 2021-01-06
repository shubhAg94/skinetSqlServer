import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  // It needs to be provided by our root and it's injected into our app module at startup
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountServie: AccountService, private router: Router){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean>{

    /*
    Then when we activate the routes and we want to observe something or we want to check what's inside
    that observable we don't actually need to subscribe because the router is going to subscribe for us
    and therefore unsubscribe from this as well.
     */
    return this.accountServie.currentUser$.pipe(
      map(auth => {
        if (auth){
          return true;
        }
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
      })
    );
  }
}
