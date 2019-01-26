import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, NgZone } from '@angular/core';
import { HttpModule, Http, Response } from '@angular/http';
import { ModalDirective } from 'ngx-bootstrap';
import { EventServiceProxy, CreateEventInput, EventLocation, EventGeoComponent } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';
import { Observable } from "rxjs/Rx"
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import * as _ from "lodash";
import * as moment from 'moment';

@Component({
    selector: 'create-event-modal',
    templateUrl: './create-event.component.html'
})


export class CreateEventComponent extends AppComponentBase {

    @ViewChild('createEventModal') modal: ModalDirective;
    @ViewChild('modalContent') modalContent: ElementRef;
    @ViewChild('eventDate') eventDate: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    event: CreateEventInput = null;
    code: number;

    

    constructor(
        injector: Injector,
        private _eventService: EventServiceProxy,
        private http: Http,
        public zone: NgZone
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
        this.event = new CreateEventInput();
        this.event.init({ isActive: true });
    }

    onShown(): void {
        $.AdminBSB.input.activate($(this.modalContent.nativeElement));
        $(this.eventDate.nativeElement).datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
    }

    save(): void {
        this.saving = true;

        this.event.date = moment($(this.eventDate.nativeElement).data('DateTimePicker').date().format('YYYY-MM-DDTHH:mm:ssZ'));
        this.event.location = new EventLocation(this.longitude, this.latitude, this.postCode, this.formattedEstablishmentAddress);

        this._eventService.createAsync(this.event)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    searchCode():void {
        this.callGeoAPI();
    }

    callGeoAPI(){debugger;
        let apiURL = `https://maps.googleapis.com/maps/api/geocode/json?address=${this.code}&key=AIzaSyAgDUII_kvGfCJNmu4qhhzjl8YNzblV9Ng`;
        //let apiURL = `https://maps.googleapis.com/maps/api/geocode/json?address=${this.code}&key=AIzaSyAuttoKc6zOBy-pt7xhl0rGWu5vh6PkD9Y`;
        //let apiURL = `http://localhost:4200`;
        return this.http.get(apiURL)
        .map((res: Response) => {
            debugger;
            return res.json().results;
        })
        .subscribe(
            data => console.log(data),
            err => console.log(err),
            () => console.log('yay')
          );
    }

    // TODO: All these shits to fall back into the component model instead of exposure like this
    address: Object;
    establishmentAddress: Object;
    formattedAddress: string;
    formattedEstablishmentAddress: string;
    phone: string;
    longitude: number;
    latitude: number;
    postCode: string;

    getAddress(place: object) {
        this.address = place['formatted_address'];
        this.phone = this.getPhone(place);
        this.formattedAddress = place['formatted_address'];
        this.zone.run(() => this.formattedAddress = place['formatted_address']);
      }
    
      getEstablishmentAddress(place: object) {
        debugger;
        this.postCode = this.getPostCode(place);
        this.address = this.establishmentAddress = place['formatted_address'];
        this.longitude = this.getGeoComponent(place, EventGeoComponent.Longitude);
        this.latitude = this.getGeoComponent(place, EventGeoComponent.Latitude);
        this.phone = this.getPhone(place);
        this.formattedEstablishmentAddress = place['formatted_address'];
        this.zone.run(() => {
          this.formattedEstablishmentAddress = place['formatted_address'];
          this.phone = place['formatted_phone_number'];
        });
      }
    
      getAddrComponent(place, componentTemplate) {
        let result;
    
        for (let i = 0; i < place.address_components.length; i++) {
          const addressType = place.address_components[i].types[0];
          if (componentTemplate[addressType]) {
            result = place.address_components[i][componentTemplate[addressType]];
            return result;
          }
        }
        return;
      }
      
      getGeoComponent(place, template: EventGeoComponent){
        //for (let i = 0; i < place.geometry.length; i++)
        return place.geometry.location[template == EventGeoComponent.Latitude ? "lat" : "lng"]();
      }
    
      getStreetNumber(place) {
        const COMPONENT_TEMPLATE = { street_number: 'short_name' },
          streetNumber = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return streetNumber;
      }
    
      getStreet(place) {
        const COMPONENT_TEMPLATE = { route: 'long_name' },
          street = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return street;
      }
    
      getCity(place) {
        const COMPONENT_TEMPLATE = { locality: 'long_name' },
          city = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return city;
      }
    
      getState(place) {
        const COMPONENT_TEMPLATE = { administrative_area_level_1: 'short_name' },
          state = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return state;
      }
    
      getDistrict(place) {
        const COMPONENT_TEMPLATE = { administrative_area_level_2: 'short_name' },
          state = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return state;
      }
    
      getCountryShort(place) {
        const COMPONENT_TEMPLATE = { country: 'short_name' },
          countryShort = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return countryShort;
      }
    
      getCountry(place) {
        const COMPONENT_TEMPLATE = { country: 'long_name' },
          country = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return country;
      }
    
      getPostCode(place) {
        const COMPONENT_TEMPLATE = { postal_code: 'long_name' },
          postCode = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return postCode;
      }
    
      getPhone(place) {
        const COMPONENT_TEMPLATE = { formatted_phone_number: 'formatted_phone_number' },
          phone = this.getAddrComponent(place, COMPONENT_TEMPLATE);
        return phone;
      }
}


