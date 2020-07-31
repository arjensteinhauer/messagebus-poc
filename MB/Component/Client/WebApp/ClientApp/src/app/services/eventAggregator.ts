import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { filter, pluck, share } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EventAggregator {
  private subject: Subject<any>;
  constructor() {
    this.subject = new Subject();
  }
  publish<T>(type: any, data: any) {
    this.subject.next({ type, data });
  }
  listen(type: any): Observable<any> {
    return this.subject
      .pipe(filter(x => x.type === type))
      .pipe(pluck('data'))
      .pipe(share());
  }
}
