import { HttpClient } from '@angular/common/http';
import { User } from './../_models/user';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
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
    this.http.get('https://localhost:5001/api/users').subscribe(users => this.users = users);
  }

  public cancelRegisterMode(event: boolean)
  {
    this.registerMode=event;
  }

}
