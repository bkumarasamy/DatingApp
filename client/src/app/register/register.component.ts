import { Router } from '@angular/router';
import { AccountService } from './../_services/account.service';
import { Component, Input, OnInit, Output,EventEmitter} from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
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
// model:any = {};
registerForm:FormGroup;
@Input() label:string;
@Input() maxDate:Date;
bsConfig: Partial<BsDatepickerConfig>;
validationErrors: string[]=[];

  constructor(private accountService:AccountService,private toastr:ToastrService,
    private fb:FormBuilder,private router:Router) {
     
     }

  ngOnInit(): void {
    this.initializeForm();
    this.bsConfig={
      containerClass:'theme-red',
      dateInputFormat:'DD MMMM YYYY',
    }
    this.maxDate=new Date();
    // this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
    this.maxDate.setDate(this.maxDate.getDate());
  }

  initializeForm(){
    this.registerForm=this.fb.group({
      gender:['male'],
      username:['',Validators.required],
      knownus:['',Validators.required],
      dateOfBirth:['',Validators.required],
      city:['',Validators.required],
      country:['',Validators.required],
      password:['',[Validators.required,
        Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword:['',[Validators.required,this.matchValues('password')]]
    })
    this.registerForm.controls.password.valueChanges.subscribe(() =>{
        this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo:string):ValidatorFn{
    return (control:AbstractControl) => {
      return control?.value===control?.parent?.controls[matchTo].value
        ?null:{isMatching:true}
    }
  }

  register(){
    // console.log(this.registerForm.value);
    this.accountService.Register(this.registerForm.value).subscribe(response=>{
      this.router.navigateByUrl('/members')
      // this.cancel();
    },error=>{
      this.validationErrors=error;
      // console.log(error);
      // this.toastr.error(error.error);
      
    })
  }

  cancel(){
    // console.log("cancelled");
    this.cancelRegister.emit(false);
  }

}
