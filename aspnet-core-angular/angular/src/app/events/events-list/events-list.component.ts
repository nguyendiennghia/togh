import { Component, Injector } from '@angular/core';
import {EventsComponentBase} from '../events.component';
import { EventServiceProxy, EntityDtoOfGuid } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.scss']
})
export class EventsListComponent extends EventsComponentBase {

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
}
