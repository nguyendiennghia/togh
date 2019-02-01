import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { GoogleMapsAPIWrapper, AgmMap, LatLngBounds, LatLngBoundsLiteral } from '@agm/core';
import { EventLocation } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'gmap',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  lat: number = 51.673858;
  lng: number = -1.815982;
  zoom: number = 12;

  @Input() location: EventLocation = new EventLocation(0, 0, '', '');

  // @ViewChild('AgmMap') agmMap: AgmMap;

  constructor() { 
  }

  ngOnInit() {
    //this.address = this.location.address;
  }

  //ngAfterViewInit() {
    /*this.agmMap.mapReady.subscribe(map => {
      const bounds: google.maps.LatLngBounds = new google.maps.LatLngBounds();
      // for (const mm of this.markers) {
      //   bounds.extend(new google.maps.LatLng(mm.lat, mm.lng));
      // }
      map.fitBounds(bounds);
    });*/
  //}

  // clickedMarker(label: string, index: number) {
  //   console.log(`clicked the marker: ${label || index}`)
  // }

  // markerDragEnd(m: marker, $event: MouseEvent) {
  //   console.log('dragEnd', m, $event);
  // }

  // markers: marker[] = [
	//   {
	// 	  lat: 51.673858,
	// 	  lng: 7.815982,
	// 	  label: 'A', //
	// 	  draggable: true
	//   },
	//   {
	// 	  lat: 51.373858,
	// 	  lng: 7.215982,
	// 	  label: 'B',
	// 	  draggable: false
	//   },
	//   {
	// 	  lat: 51.723858,
	// 	  lng: 7.895982,
	// 	  label: 'C',
	// 	  draggable: true
	//   }
  // ]

}

// interface marker {
// 	lat: number;
// 	lng: number;
// 	label?: string;
// 	draggable: boolean;
// }
