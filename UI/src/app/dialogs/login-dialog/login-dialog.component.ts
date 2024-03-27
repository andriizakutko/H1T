import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/auth/auth.service';
import { Login } from 'src/app/models/Login';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent {
  hidePass = true;
  loginModel: Login = {
    email: '',
    password: ''
  };

  loginFormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  constructor(
    private formBuilder: FormBuilder, 
    private auth: AuthService, 
    private toasrt: ToastrService,
    private loginDialogRef: MatDialogRef<LoginDialogComponent>) {}

  resetLoginForm() {
    this.loginFormGroup.reset();
  }

  login() {
    if(this.loginFormGroup.valid) {
      this.loginModel.email = this.loginFormGroup.controls.email.value!;
      this.loginModel.password = this.loginFormGroup.controls.password.value!;

      this.auth.login(this.loginModel).subscribe(response => {
        if(response.statusCode === 200) {
          this.auth.authProcess(response);
          this.toasrt.success("You have successfully logged in");
          this.loginDialogRef.close();
        }
        if(response.statusCode === 400) {
          this.toasrt.error((response.data as any).message);
        }
      });
    }
    else {
      console.log(this.loginFormGroup.get('email')!.errors)
    }
  }
}
