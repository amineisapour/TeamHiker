import { MediaMatcher } from '@angular/cdk/layout';
import { ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { MenuService } from 'src/app/services/menu.service';

@Component({
  selector: 'app-admin-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnDestroy {

  @ViewChild('snav') snav: any;

  mobileQuery: MediaQueryList;

  private _mobileQueryListener: () => void;

  constructor(
    changeDetectorRef: ChangeDetectorRef, 
    media: MediaMatcher,
    private menuService: MenuService
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.menuService.closeMenu$.subscribe(() => this.closeMenu());
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  closeMenu() {
    // this.snav.toggle();
    //alert(this.mobileQuery.matches);
    if (this.snav && this.mobileQuery.matches) {
      this.snav.close(); // Close the sidenav
    }
  }

}
