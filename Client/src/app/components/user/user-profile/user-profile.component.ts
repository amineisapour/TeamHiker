import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CurrentUser } from 'src/app/models/current-user.model';
import { AccountService } from 'src/app/services/account.service';
import { DateTimeFormat, Gender, LoaderColor, LoaderType, MessageType } from 'src/app/models/enums/enums';
import { AppDateAdapter, AppDateTime, APP_DATE_FORMATS } from 'src/app/infrastructure/helpers/format-datepicker.helper';
import { ValidationService } from 'src/app/services/common/validation.service';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatChipInputEvent} from '@angular/material/chips';

export interface CertificationList {
  name: string;
}

export interface SocialMediaList {
  name: string;
}

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  public currentUser: CurrentUser;
  public registerForm: FormGroup;
  
  userGender = Gender;

  languageList: string[] = ['English', 'Chinese', 'Hindi', 'Spanish', 'French', 'Arabic', 'Bengali', 'Portuguese', 'Russian', 'German', 'Japanese', 'Turkish', 'Persian (Farsi, Dari, Tajik)', 'Indonesian'];

  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  //#region Certifications
  selectable = true;
  removable = true;
  addOnBlur = true;

  certificationList: CertificationList[] = [
    // {name: 'Lemon'},
    // {name: 'Lime'},
    // {name: 'Apple'},
  ];

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

  socialMediaList: SocialMediaList[] = [
    // {name: 'Lemon'},
    // {name: 'Lime'},
    // {name: 'Apple'},
  ];

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
  ) {
    this.currentUser = this.accountService.getCurrentUser();
    this.registerForm = this.formBuilder.group({
      gender: ['', [Validators.required]],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      bio: ['',[Validators.required]],
      language: ['', [Validators.required]],
      certification: ['', []],
      email: ['', [Validators.required, ValidationService.emailValidator]],
      phone: ['', [Validators.required]],
      socialMediaLinks: ['', []],
      equipment: ['', []],
    });
  }

   register(data: any): void {
       const model = {
         "Username": data.username,
         "Password": data.password,
         "FirstName": data.firstName,
         "LastName": data.lastName,
         "NationalId": data.nationalId,
         "Gender": (data.gender == Gender.Man) ? 1 : 0,
         "Birthdate": AppDateTime.getFormatDateTime(data.birthdate, DateTimeFormat.YyyyMmDd)
       };
       this.accountService.register(model).subscribe(
       );
     }

  ngOnInit(): void { }

  getErrorMessage(element: string): string {
    return this.registerForm.controls[element].hasError('required') ? 'The ' + element + ' is required!' :
      this.registerForm.controls[element].hasError('invalidEmailAddress') ? 'Not a valid ' + element + '!' :
        this.registerForm.controls[element].hasError('invalidPassword') ? 'At least 8 characters long including uppercase, lowercase, numeric, and special character.' :
          this.registerForm.controls[element].hasError('invalidNationalId') ? 'National ID must be a number and min length and max length must be 10!' :
            this.registerForm.controls[element].hasError('pattern') ? 'Passwords do not match' :
              '';
  }
}
