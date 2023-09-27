import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatAccordion } from '@angular/material/expansion';
import { Router } from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-main-navigation',
  templateUrl: './main-navigation.component.html',
  styleUrls: ['./main-navigation.component.scss']
})
export class MainNavigationComponent implements OnInit {
  @ViewChild(MatAccordion) accordion: MatAccordion = new MatAccordion();

  @Output() toggleSidenav = new EventEmitter();

  public menuList : any;

  public typeUser: any = localStorage.getItem('typeUser');
  happyBirhday = false;
  constructor(
    private router: Router,
    ) { }

  ngOnInit(): void {
  /**
   * Menú harcodeado
   **/
    this.menuList = [
      {
        field: 'Sistema de eventos',
        icon: 'campaign',
        rol: ['ROOT', '**'],
        childs: [
          {
            field: 'Nuevo evento',
            path: '/events',
            rol: ['ROOT', 'EMPLOYEE'],
            new: true
          },
          {
            field: 'Todos los eventos',
            path: '/events/list',
            rol: ['ROOT', 'EMPLOYEE'],
          },
        ],
      },
    ]
  }
}
