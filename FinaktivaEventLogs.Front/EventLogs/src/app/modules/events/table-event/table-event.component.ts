import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortable, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { GlobalService } from 'projects/shared/src/lib/services/global.service';
import { SharedService } from 'projects/shared/src/public-api';

@Component({
  selector: 'app-table-event',
  templateUrl: './table-event.component.html',
  styleUrls: ['./table-event.component.css']
})
export class TableEventComponent implements OnInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  // Tipos de eventos para contadores
  countsAssetType = [];
  /**
   * Inicio sección para filtros
   */
  // Form filter
  FormGroupFilter: FormGroup;
  FormArrayFilters: FormArray;
  numberFilters = [];
  filtersCount = 0;
  filterValue: {
    key: string;
    filterValues: Array<any>;
  } = {
    key: '',
    filterValues: [],
  };
  // opciones de filtrado
  filterOptions: Array<any> = [];
  /**
   * Fin sección para filtros
   */
  // MatPaginator Output
  pageEvent!: PageEvent;
  // MatPaginator Inputs
  length = 0;
  pageSize = 10;

  //  Array para datos de la tabla
  dataTable = [];
  // dataSource: MatTableDataSource<MembershipData>;
  dataSource: MatTableDataSource<any>;

  displayedColumns: string[] = [
    'position',
    'title',
    'type',
    'description',
  ];
  constructor(
    private globalService: GlobalService,
    private formBuilder: FormBuilder,
    private sharedService: SharedService,

  ) {
    // Assign the data to the data source for the table to renderusers
    this.dataSource = new MatTableDataSource();
    // Controls FormGroupFilter
    this.FormGroupFilter = this.formBuilder.group({
      FormArrayFilters: new FormArray([
        new FormGroup({
          FormControlFilterBy: new FormControl(null, [
            Validators.required,
          ]),
          FormControlFilterString: new FormControl(null),
          FormControlFilterFrom: new FormControl(null),
          FormControlFilterTo: new FormControl(null),
          FormControlFilterSelect: new FormControl(null),
          FormControlTypeUnionFilter: new FormControl(null),
        }),
      ]),
    });
    this.FormArrayFilters = this.FormGroupFilter.controls["FormArrayFilters"] as FormArray;
   }

  ngOnInit() {
    this.filterOptions = [
    {
      label: 'Tipo',
      value: 'type',
      type: 'select',
      options: [
        { viewValue: 'Api', value: '0' },
        { viewValue: 'Formulario', value: '1' },
      ],
    },
    { label: 'Fecha de registro', value: 'createRegisterDate', type: 'date' },
    { label: 'Description', value: 'description', type: 'text' },
    { label: 'Title', value: 'title', type: 'text' },
  ];
}
ngAfterViewInit() {
  // Se inicializan paginator y sort
  this.dataSource.paginator = this.paginator;
  this.sort.sort({ id: 'createRegisterDate', start: 'desc' } as MatSortable);
  this.dataSource.sort = this.sort;
  // Suscripción a cambios en paginación
  this.paginator.page.subscribe({
    next: (pagChange: { pageSize: any; pageIndex: any; }) => {
      // consulta a base de datos con paginación
      this.getEvents(
        pagChange.pageSize,
        pagChange.pageIndex,
        this.sort.active,
        this.sort.direction,
        this.filterValue.key,
        this.filterValue.filterValues
      );
    },
    error: (error: any) => {
      console.log('error', error);
    },
  });
}
/**
   * Primera consulta para ordenar tabla
   * @param sort ordenamiento as MatSort
   * ({ id: 'createRegisterDate', start: 'desc' } as MatSortable);
   */
sortData(sort: Sort) {
  // Filtrar por el sort enviado
  if (
    sort.active !== '' &&
    sort.direction !== ''
  ) {
    // consulta a base de datos con nuevo orden
    this.getEvents(
      this.paginator.pageSize,
      0,
      this.sort.active,
      this.sort.direction,
    );
  }
}
/**
 * Controles de Forms Arrays, aumentar o disminuir formsArrays
 * @param type FormArray a modificar
 * @param number Elementos a sumar o restar
 * @param index ? Si se elimina indicar que elemento se eliminará
 * @returns
 */
changeControlsFormArray(type: number, number: number, index?: number) {
  switch (type) {
    case 0:
      if (number > 0) {
        this.FormArrayFilters.push(
          new FormGroup({
            FormControlFilterBy: new FormControl(null, [
              Validators.required,
            ]),
            FormControlFilterString: new FormControl(null),
            FormControlFilterFrom: new FormControl(null),
            FormControlFilterTo: new FormControl(null),
            FormControlFilterSelect: new FormControl(null),
            FormControlTypeUnionFilter: new FormControl(null),
          })
        );
      } else if (index && this.FormArrayFilters) {
        this.numberFilters.splice(index, 1);
        this.FormArrayFilters.removeAt(index);
        if (index > 0 && this.FormArrayFilters
           ) {
          this.FormArrayFilters.controls[index - 1]
            .get('FormControlTypeUnionFilter')
            .setValue(null);
        }
      }
      this.getCountFilters();
      break;
  }
}
/**
 * Cuenta los filtros actuales y los espacios para los valores a almacenar
 */
getCountFilters() {
  this.filtersCount = 0;
  this.numberFilters.forEach((numberFilter) => {
    this.filtersCount += numberFilter;
  });
}
/**
 * Busqueda rápida por Frontend sin consulta sólo en los resultados cargados
 * @param event evento en frontend en filerfast
 */
applyFilterFast(event: Event) {
  const filterValue = (event.target as HTMLInputElement).value;
  this.dataSource.filter = filterValue.trim();
  if (this.dataSource.paginator) {
    this.dataSource.paginator.firstPage();
  }
}

/**
 * Filtrar con consulta a Backend
 */
applyFilter() {
  // marcar como touched todos los form para mostrar errores
  this.FormGroupFilter.markAllAsTouched();
  if (this.FormGroupFilter.valid) {
    const formValues = this.FormGroupFilter.value.FormArrayFilters;
    /**
     * se recorre los filtros creados y se crea la cadena de consulta
     * asignando un comodin para dinamic linq y asignando su valor en el arreglo filterValues
     */
    let indexValues = 0;
    this.filterValue.key = '';
    this.filterValue.filterValues = [];
    formValues.forEach((filter : any) => {
      const filterOption = this.filterOptions.find(
        (f) => f.value === filter.FormControlFilterBy
      );
      switch (filterOption.type) {
        // rangos
        case 'date':
          // Columna y expresion de filtrado rango
          this.filterValue.key += `(${
            filter.FormControlFilterBy
          } >= @${indexValues} AND ${filter.FormControlFilterBy} <= @${
            indexValues + 1
          })`;
          // EL menor valor para control desde
          this.filterValue.filterValues[indexValues] =
            filter.FormControlFilterFrom < filter.FormControlFilterTo
              ? filter.FormControlFilterFrom
              : filter.FormControlFilterTo;
          // El mayor valor para control hasta
          this.filterValue.filterValues[indexValues + 1] =
            filter.FormControlFilterFrom > filter.FormControlFilterTo
              ? filter.FormControlFilterFrom
              : filter.FormControlFilterTo;
          indexValues += 2;
          break;
        // Ranges numbers
        case 'number':
          // Columna y expresion de filtrado rango
          this.filterValue.key += `(${
            filter.FormControlFilterBy
          } >= @${indexValues} AND ${filter.FormControlFilterBy} <= @${
            indexValues + 1
          })`;
          // EL menor valor para control desde
          const min = Math.min(
            filter.FormControlFilterFrom,
            filter.FormControlFilterTo
          );
          // El mayor valor para control hasta
          const max = Math.max(
            filter.FormControlFilterFrom,
            filter.FormControlFilterTo
          );
          this.filterValue.filterValues[indexValues] = min;
          this.filterValue.filterValues[indexValues + 1] = max;
          indexValues += 2;
          break;
        case 'text':
          // Columna y expresion de filtrado Contains para cadenas
          this.filterValue.key += `(${filter.FormControlFilterBy}.Contains(@${indexValues}))`;
          this.filterValue.filterValues[indexValues] =
            filter.FormControlFilterString;
          indexValues++;
          break;
        // Selects enums
        case 'select':
          // Columna y expresion de filtrado para Enums
          this.filterValue.key += `(${filter.FormControlFilterBy} == @${indexValues})`;
          this.filterValue.filterValues[indexValues] =
            filter.FormControlFilterSelect;
          indexValues++;
          break;
      }
      // añade la unión de filtros AND / OR
      // si hay más de un filtro
      if (formValues.length > 1 && filter.FormControlTypeUnionFilter != null)
        this.filterValue.key += ` ${filter.FormControlTypeUnionFilter} `;
    });
    this.getEvents(
      this.paginator.pageSize,
      0,
      this.sort.active,
      this.sort.direction,
      this.filterValue.key,
      this.filterValue.filterValues
    );
    if (this.dataSource.paginator) this.dataSource.paginator.firstPage();
  }
}

/**
 * Método para filtrar por contadores circulares
 * @param type Nombre de la columna filtro
 * @param filterType valor a filtrar
 */
filterByButton(type: string, filterType: string) {
  this.filterValue = {
    key: `${type} == @0`,
    filterValues: [filterType],
  };
  this.getEvents(
    this.paginator.pageSize,
    0,
    this.sort.active,
    this.sort.direction,
    this.filterValue.key,
    this.filterValue.filterValues
  );
}

/**
 * Método para borrar uno o todos los filtros
 * @param all Limpiar todos los filtros?
 * @param indexFilter Eliminar un solo filtro
 */
resetFilter(all: boolean, indexFilter?: number) {
  if (all) {
    this.filterValue = {
      key: '',
      filterValues: [],
    };
    this.FormArrayFilters.clear();
    this.changeControlsFormArray(0, 1);
    this.getEvents();
  } else {
    this.FormArrayFilters.controls[indexFilter as number]
      .get('FormControlFilterString')
      .reset();
    this.FormArrayFilters.controls[indexFilter]
      .get('FormControlFilterFrom')
      .reset();
    this.FormArrayFilters.controls[indexFilter]
      .get('FormControlFilterTo')
      .reset();
    this.FormArrayFilters.controls[indexFilter]
      .get('FormControlFilterSelect')
      .reset();
    this.FormArrayFilters.controls[indexFilter]
      .get('FormControlTypeUnionFilter')
      .reset();
    switch (
      this.filterOptions.find(
        (f) =>
          f.value ===
          this.FormArrayFilters.controls[indexFilter].get(
            'FormControlFilterBy'
          ).value
      ).type
    ) {
      // rangos
      case 'date':
      case 'number':
        this.numberFilters[indexFilter] = 2;
        break;
      case 'text':
      case 'select':
        this.numberFilters[indexFilter] = 1;
        break;
    }
  }
  this.getCountFilters();
  //Detectar cambios de interfaz, cuando habilita opciones de pdf
  this.changeDetector.detectChanges();
}
}
