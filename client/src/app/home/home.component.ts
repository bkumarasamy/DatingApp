import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from './../_models/user';
import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

const httpOptions={
  headers:new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  baseUrl=environment.apiUrl;  
  registerMode=false;
  users:any;
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getuser();
  }

  registerToggle(){
    this.registerMode=!this.registerMode;
  }

  getuser(){
    this.http.get(this.baseUrl+'users',httpOptions).subscribe(users => this.users = users);
    // this.http.get('https://localhost:5001/api/users',httpOptions).subscribe(users => this.users = users);
  }

  public cancelRegisterMode(event: boolean)
  {
    this.registerMode=event;
  }

}
