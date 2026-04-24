import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, User } from '../../shared/models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseurl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<User>(this.baseurl + 'login', values, { params });
  }

  register(values: any) {
    return this.http.post<User>(this.baseurl + 'account/register', values);
  }

  getUserInfo(){
    return this.http.get<User>(this.baseurl + 'account/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    );
  }

  logout(){
    return this.http.post(this.baseurl + 'account/logout', {});
  }

  updateAddress(address: Address){
    return this.http.post<Address>(this.baseurl + 'account/address', address);
  }

  getAuthState(){
    return this.http.get<{ isAuthenticated: boolean }>(this.baseurl + 'account/auth-status');
  }

}
