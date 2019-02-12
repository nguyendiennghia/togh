import { Component, Injector, ViewChild } from '@angular/core';
import {EventsComponentBase} from '../events.component';
import { EventServiceProxy, EntityDtoOfGuid } from '@shared/service-proxies/service-proxies';
import { EventSearchComponent } from '@app/events/event-search/event-search.component';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.scss']
})
export class EventsListComponent extends EventsComponentBase {

  @ViewChild('eventSearch') searchComponent: EventSearchComponent;

  constructor(
    injector: Injector,
    _eventService: EventServiceProxy
  ) {
      super(injector, _eventService);
  }

  protected delete(event: EntityDtoOfGuid): void {
  }

  createEvent(): void {
    
  }

  // public getDataPage(page: number): void {
  //   // this.list(req, page, () => {
  //   //     this.isTableLoading = false;
  //   // });
  //   this.events = this.searchComponent.events;
  // }
}
