/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TableEventComponent } from './table-event.component';

describe('TableEventComponent', () => {
  let component: TableEventComponent;
  let fixture: ComponentFixture<TableEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
