import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {switchMap, debounceTime, tap, finalize} from 'rxjs/operators';
import {Observable} from 'rxjs';
import {EventService} from '../event-service.service';
import { EventListDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'event-search',
  templateUrl: './event-search.component.html',
  styleUrls: ['./event-search.component.scss']
})
export class EventSearchComponent implements OnInit {

  data: string = '';
  form: FormGroup;
  loading: boolean = false;
  events: EventListDto[];

  constructor(private fb: FormBuilder, private service: EventService) { }

  ngOnInit() {
    this.form = this.fb.group({searchInput: null});

    this.form
      .get('searchInput')
      .valueChanges
      .pipe(
        debounceTime(400),
        tap((v: string) => this.loading = v!==""),
        // TODO: More criteria to come into filter
        switchMap(value => this.service.search({f: value})
          .pipe(
            finalize(() => this.loading = false)
          )
        )

        // switchMap(value => this.service.search({f: value})
        // .pipe(
        //   finalize(() => this.loading = false),
        //   )
        // )
      )
      .subscribe(events => this.events = events.items);
  }

  search(): void {
    var that: any = this;
    setTimeout(function() {
      alert(that.data);
    }, 400);
    
  }

}
