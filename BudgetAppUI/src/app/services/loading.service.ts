import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoadingSubject.asObservable();

  public showLoading() {
    this.isLoadingSubject.next(true);
  }

  public hideLoading() {
    this.isLoadingSubject.next(false);
  }
}
