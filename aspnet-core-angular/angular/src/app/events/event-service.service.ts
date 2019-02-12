import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { EventListDto, EventServiceProxy, ListResultDtoOfEventListDto } from '@shared/service-proxies/service-proxies';
import {map, tap, debounceTime} from 'rxjs/operators';

@Injectable()
export class EventService {

  constructor(private http: HttpClient, private _eventService: EventServiceProxy) { }

  // TODO: Detect after selecting category or text or ... ?
  search(filter: {f: string} = {f: ''}): Observable<ListResultDtoOfEventListDto> {
    // TODO: Naive search
    return this._eventService.getListAsync(false)
      .map(list => {
        list.items = list.items.filter(e => e.description.includes(filter.f));
        return list;
      });
  }
}

export interface IEventResponse {
  total: number;
  results: EventListDto[];
}
