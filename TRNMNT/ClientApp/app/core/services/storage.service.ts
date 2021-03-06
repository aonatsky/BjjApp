import { Injectable, OnDestroy } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { share, filter, map } from 'rxjs/operators';
import { BracketModel, RefreshBracketModel } from '../model/bracket.models';

@Injectable()
export class StorageService implements OnDestroy {
  private onSubject = new Subject<{ key: string; value: any }>();
  public changes = this.onSubject.asObservable().pipe(share());

  constructor() {
    this.start();
  }

  ngOnDestroy() {
    this.stop();
  }

  public getStorage() {
    let s = [];
    for (let i = 0; i < localStorage.length; i++) {
      s.push({
        key: localStorage.key(i),
        value: JSON.parse(localStorage.getItem(localStorage.key(i)))
      });
    }
    return s;
  }

  public getObject(key: string) {
    const data = localStorage.getItem(key);
    if (!!data) {
      return JSON.parse(data);
    }
  }

  public store(key: string, data: any): void {
    localStorage.setItem(key, JSON.stringify(data));
    // the local application doesn't seem to catch changes to localStorage...
    this.onSubject.next({ key: key, value: data });
  }

  public clear(key) {
    localStorage.removeItem(key);
    // the local application doesn't seem to catch changes to localStorage...
    this.onSubject.next({ key: key, value: null });
  }

  public observeByKey<T>(key): Observable<T> {
    return this.changes.pipe(
      filter(data => data.key === key),
      map(data => data.value)
    );
  }

  private start(): void {
    window.addEventListener('storage', this.storageEventListener.bind(this));
  }

  private storageEventListener(event: StorageEvent) {
    if (event.storageArea == localStorage) {
      let v;
      try {
        v = JSON.parse(event.newValue);
      } catch (e) {
        v = event.newValue;
      }
      this.onSubject.next({ key: event.key, value: v });
    }
  }

  private stop(): void {
    window.removeEventListener('storage', this.storageEventListener.bind(this));
    this.onSubject.complete();
  }
}
