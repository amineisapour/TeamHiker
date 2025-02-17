import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private closeMenuSubject = new Subject<void>();
  closeMenu$ = this.closeMenuSubject.asObservable();

  triggerCloseMenu() {
    this.closeMenuSubject.next();
  }
}