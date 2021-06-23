import { AccountService } from './../_services/account.service';
import { Component, Input, OnInit, Output,EventEmitter} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
// import * as  from 'events';

//import { EventEmitter } from 'stream';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
// @Input() usersFromHomeComponent:any;  
@Output() cancelRegister=new EventEmitter();
model:any = {};
  constructor(private accountService:AccountService,private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    console.log(this.model);
    this.accountService.Register(this.model).subscribe(response=>{
      console.log(response);
      this.cancel();
    },error=>{
      console.log(error);
      this.toastr.error(error.error);
      
    })
  }

  cancel(){
    // console.log("cancelled");
    this.cancelRegister.emit(false);
  }

}
