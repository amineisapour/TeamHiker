import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, Params } from '@angular/router';
//import { HttpRequestResult } from 'src/app/interfaces/http-request-result.interface';
import { HttpRequestResult } from 'src/app/models/http-request-result.model';
import { AuthenticateData } from 'src/app/models/authenticate-data.model';
import { CurrentUser } from 'src/app/models/current-user.model';
import { LocalStorageData } from 'src/app/models/local-storage-data.model';
import { AccountService } from 'src/app/services/account.service';
import { LocalStorageService } from 'src/app/services/common/local-storage.service';
import { ValidationService } from 'src/app/services/common/validation.service';
import { SnackbarComponent } from 'src/app/components/common/snackbar/snackbar.component';
import { LoaderColor, LoaderType, MessageType } from 'src/app/models/enums/enums';
import { ErrorHandleHelper } from 'src/app/infrastructure/helpers/error-handle.helper';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public loginForm: FormGroup;
  public returnUrl: string = '/';
  public hide = true;
  loaderType = LoaderType;
  loaderColor = LoaderColor;
  //myInfo$ = this.localStorageService.myData$;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private localStorageService: LocalStorageService,
    public snackbar: SnackbarComponent
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, ValidationService.emailValidator]],
      password: ['', [Validators.required, ValidationService.passwordValidator]]
    });
  }

  ngOnInit(): void {
    if (this.accountService.isAuthenticat()) {
      this.router.navigate(['/']);
      return;
    }

    this.localStorageService.clearAllLocalStorage();

    // let returnUrl = this.activatedRoute.snapshot.params.returnUrl;
    // if (returnUrl != null) {
    //   this.returnUrl = returnUrl;
    // }

    this.activatedRoute.queryParams.subscribe((params: Params) => {
      let retUrl = params['returnUrl'];
      if (retUrl != undefined && retUrl != null && retUrl != '') {
        this.returnUrl = params['returnUrl'];
      }
    });

    // this.activatedRoute.params.subscribe((params: Params) => {
    //   if (params.returnUrl) {
    //     this.returnUrl = params.returnUrl;
    //   }
    // });
  }

  getErrorMessage(element: string): string {
    return this.loginForm.controls[element].hasError('required') ? 'The ' + element + ' is required!' :
      this.loginForm.controls[element].hasError('invalidEmailAddress') ? 'Not a valid ' + element + '!' :
        this.loginForm.controls[element].hasError('invalidPassword') ? 'At least 8 characters long including uppercase, lowercase, numeric, and special character.' :
          '';
  }

  login(data: any): void {
    const model = {
      "username": data.username,
      "password": data.password
    };
    this.accountService.login(model).subscribe(
      (result: HttpRequestResult<AuthenticateData>) => {
        //console.log(result);
        if (result.isFailed) {
          // result.errors.forEach(function (item, index) {
          //   console.log(item);
          // });
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        } else {
          if (result.value != null) {
            let user = new CurrentUser(
              result.value.id,
              result.value.username,
              result.value.gender,
              result.value.fullName
            );
            this.localStorageService.setInfo(new LocalStorageData<CurrentUser>("current-user", user));
            this.localStorageService.setInfo(new LocalStorageData<string>("token", result.value.token));
            this.localStorageService.setInfo(new LocalStorageData<string>("refresh-token", result.value.refreshToken));

            window.location.href = this.returnUrl;
          } else {
            //console.log('problem!');
            this.snackbar.openSnackBar('problem!', MessageType.Error);
          }
        }
      },
      (error: HttpErrorResponse) => {
        return ErrorHandleHelper.handleError(error, this.snackbar);
      }
    );
  }

  register(): void {
    this.router.navigate(['/auth/register']);
  }

}
