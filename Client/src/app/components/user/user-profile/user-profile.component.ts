import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CurrentUser } from 'src/app/models/current-user.model';
import { AccountService } from 'src/app/services/account.service';
import { DateTimeFormat, Gender, LoaderColor, LoaderType, MessageType } from 'src/app/models/enums/enums';
import { AppDateAdapter, AppDateTime, APP_DATE_FORMATS } from 'src/app/infrastructure/helpers/format-datepicker.helper';
import { ValidationService } from 'src/app/services/common/validation.service';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatChipInputEvent} from '@angular/material/chips';
import { HttpRequestResult } from 'src/app/models/http-request-result.model';
import { User } from 'src/app/models/users/user.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandleHelper } from 'src/app/infrastructure/helpers/error-handle.helper';
import { SnackbarComponent } from '../../common/snackbar/snackbar.component';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';

export interface CertificationList {
  name: string;
}

export interface SocialMediaList {
  name: string;
}

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
    providers: [
      { provide: DateAdapter, useClass: AppDateAdapter },
      { provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS }
    ]
})
export class UserProfileComponent implements OnInit {

  public currentUser: CurrentUser;
  public registerForm: FormGroup;
  
  //userGender = Gender;
  // userGender = {
  //   Woman: Gender.Woman,
  //   Man: Gender.Man
  // };
  userGender = [
    { key: 'Woman', value: 0 },
    { key: 'Man', value: 1 }
  ];
  public selectedId: number;

  languageList: string[] = ['English', 'Chinese', 'Hindi', 'Spanish', 'French', 'Arabic', 'Bengali', 'Portuguese', 'Russian', 'German', 'Japanese', 'Turkish', 'Persian', 'Indonesian'];

  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  //#region Certifications
  selectable = true;
  removable = true;
  addOnBlur = true;

  certificationList: CertificationList[] = [];

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our Certification
    if (value) {
      this.certificationList.push({name: value});
    }

    // Clear the input value
    event.chipInput!.clear();
  }

  remove(certification: CertificationList): void {
    const index = this.certificationList.indexOf(certification);

    if (index >= 0) {
      this.certificationList.splice(index, 1);
    }
  }
  //#endregion

  //#region socialMediaLinks
  selectable_socialMedia = true;
  removable_socialMedia = true;
  addOnBlur_socialMedia = true;

  socialMediaList: SocialMediaList[] = [];

  add_socialMediaLinks(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our Social Media
    if (value) {
      this.socialMediaList.push({name: value});
    }

    // Clear the input value
    event.chipInput!.clear();
  }

  remove_socialMediaLinks(socialMedia: SocialMediaList): void {
    const index = this.socialMediaList.indexOf(socialMedia);

    if (index >= 0) {
      this.socialMediaList.splice(index, 1);
    }
  }
  //#endregion

  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder,
    public snackbar: SnackbarComponent
  ) {
   
  }

  register(data: any): void {
    //console.log(data);
    //console.log(this.socialMediaList);
    //console.log(this.certificationList);
    const model = {
      "FirstName": data.firstName,
      "LastName": data.lastName,
      "Gender": data.gender,
      "Birthdate": AppDateTime.getFormatDateTime(data.birthdate, DateTimeFormat.YyyyMmDd),
      "Bio": data.bio,
      "Email": data.email,
      "Phone": data.phone,
      "Equipment": data.equipment,
      "SocialMediaLinks": this.socialMediaList.map(item => item.name).join(', '),
      "Certifications": this.certificationList.map(item => item.name).join(', '),
      "Language": data.language.join(', ')
    };

    console.table(model);
    console.log(model);
  //  this.accountService.register(model).subscribe(
  //  );
  }

  getErrorMessage(element: string): string {
    return this.registerForm.controls[element].hasError('required') ? 'The ' + element + ' is required!' :
      this.registerForm.controls[element].hasError('invalidEmailAddress') ? 'Not a valid ' + element + '!' :
        this.registerForm.controls[element].hasError('invalidPassword') ? 'At least 8 characters long including uppercase, lowercase, numeric, and special character.' :
          this.registerForm.controls[element].hasError('invalidNationalId') ? 'National ID must be a number and min length and max length must be 10!' :
            this.registerForm.controls[element].hasError('pattern') ? 'Passwords do not match' :
              '';
  }

  ngOnInit(): void { 
    this.currentUser = this.accountService.getCurrentUser();

    let id = this.currentUser.id;
    
    this.accountService.getUser(id).subscribe(
      (result: HttpRequestResult<User>) => {
        if (result.isFailed) {
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        }
        else {
          if (result.value != null) {
            let user = result.value;

            // console.table(user);
            // console.log(user);

            this.selectedId = user.gender;
            //let ln = 'Hindi, Spanish, Persian';
            let languageArray = (user.language != null) ?
                                user.language.split(',').map(lang => lang.trim())  :
                                [];
            let mediaList = 'https://www.linkedin.com/in/amin-eisapour/, https://x.com/AminEisapour, https://www.instagram.com/amin_eisapour/';
            this.socialMediaList = (user.socialMediaLinks != null) ?
                                    user.socialMediaLinks.split(',').map(media => ({ name: media.trim() })) :
                                    [];
            this.certificationList = (user.certifications != null) ?
                                      user.certifications.split(',').map(cert => ({ name: cert.trim() })) :
                                      [];

            this.registerForm = this.formBuilder.group({
              gender: [user.gender, [Validators.required]],
              firstName: [user.firstName, [Validators.required, Validators.maxLength(50)]],
              lastName: [user.lastName, [Validators.required, Validators.maxLength(50)]],
              birthdate: [user.birthdate, [Validators.required]],
              bio: [user.bio,[Validators.required]],
              language: [languageArray, [Validators.required]],
              certification: [user.certifications, []],
              email: [user.email, [Validators.required, ValidationService.emailValidator]],
              phone: [user.phone, [Validators.required]],
              socialMediaLinks: [user.socialMediaLinks, []],
              equipment: [user.equipment, []],
            });
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

}
