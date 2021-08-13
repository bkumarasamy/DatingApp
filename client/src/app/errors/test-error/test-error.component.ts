import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
baseUrl=environment.apiUrl;//'https://localhost:5001/api/';
validationErrors: string[]=[];

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    this.http.get(this.baseUrl+'buggy/not-found').subscribe(response=>{
      console.log(response);
    },error =>{
      console.log(error);
    })
  }
  
  get400Error(){
    this.http.get(this.baseUrl+'buggy/bad-request').subscribe(response=>{
      console.log(response);
    },error =>{
      console.log(error);
    })
  }

  get500Error(){
    this.http.get(this.baseUrl+'buggy/server-error').subscribe(response=>{
      console.log(response);
      console.log('response');
    },error =>{
      console.log(error);
      console.log('error');
    })
  }

  get401Error(){
    this.http.get(this.baseUrl+'buggy/auth').subscribe(response=>{
      console.log(response);
      console.log('response');
    },error =>{
      console.log(error);
      console.log('error');
    })
  }

  get400validationError(){
    this.http.post(this.baseUrl+'account/register',{}).subscribe(response=>{
      console.log(response);
    },error =>{
      console.log(error);
      this.validationErrors=error;
    })
  }

}
