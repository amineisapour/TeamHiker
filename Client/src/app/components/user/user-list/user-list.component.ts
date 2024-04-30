import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DateTimeFormat, MessageType } from 'src/app/models/enums/enums';
import { HttpRequestResult } from 'src/app/models/http-request-result.model';
import { User } from 'src/app/models/users/user.model';
import { AccountService } from 'src/app/services/account.service';
import { SnackbarComponent } from '../../common/snackbar/snackbar.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ErrorHandleHelper } from 'src/app/infrastructure/helpers/error-handle.helper';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  dateTimeFormat: typeof DateTimeFormat = DateTimeFormat;
  displayedColumns: string[] = ['id', 'username', 'gender', 'birthdate', 'registerDateTime', 'isActive'];
  dataSource: MatTableDataSource<User>;

  @ViewChild(MatPaginator) paginator: MatPaginator | any;
  // @ViewChild(MatPaginator, { static: false }) set paginator(value: MatPaginator) {
  //   this.dataSource.paginator = value;
  // }

  @ViewChild(MatSort) sort: MatSort | any;
  // @ViewChild(MatSort, { static: false }) set sort(value: MatSort) {
  //   this.dataSource.sort = value;
  // }

  constructor(
    private accountService: AccountService,
    public snackbar: SnackbarComponent
  ) {
    this.dataSource = new MatTableDataSource<User>();
  }

  ngOnInit() {
    this.accountService.getAllUser().subscribe(
      (result: HttpRequestResult<User[]>) => {
        if (result.isFailed) {
          this.snackbar.openSnackBar(result.errors, MessageType.Error);
        }
        else {
          if (result.value != null) {
            let listUser = result.value;
            //console.table(listUser);
            this.dataSource = new MatTableDataSource(listUser);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
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

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  // public get DateTimeFormat(): typeof DateTimeFormat {
  //   return DateTimeFormat; 
  // }

}
