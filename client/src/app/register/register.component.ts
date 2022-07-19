import { ToastrService } from 'ngx-toastr';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      // And inside each of these form controls, what we can do is we could give it a starting value, we could give it an initial value.
      // And then what we can add, the next parameter is our validation options.
      // And what we can specify here is validators and then we can choose from the out-of-box validators that Angular provides.
      username: new FormControl('Hello', Validators.required),
      // And if we wanted to add two validators then we would put them inside an array.
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      // And then we'll need to add a custom validator so that we can compare the password field with the confirmpassword field.
      //
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn{
    return (control: AbstractControl) => {
      // What's what I'm just going to clarify now is that what we're doing here is we're getting access to the control that we're going to attach this validator to.
      //   confirm password control.
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isMatching: true}
    }
  }

  register(){
    console.log(this.registerForm.value);
    // this.accountService.register(this.model).subscribe(response => {
    //   console.log(response);
    //   this.cancel();
    // }, error => {
    //   console.log(error);
    //   this.toastr.error(error.error);
    // })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
