import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import * as moment from 'moment';
import { GlobalService } from 'projects/shared/src/lib/services/global.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-dialog-event',
  templateUrl: './dialog-event.component.html',
  styleUrls: ['./dialog-event.component.css']
})
export class DialogEventComponent implements OnInit {
  // Btn enviar formulario
  btnSubmit = true;
  // formularios
  FormGroupSend: FormGroup<any>;
  FormCreate: FormGroup<any>;

  constructor(
    private formBuilder: FormBuilder,
    private globalService: GlobalService,
    public dialog: MatDialog,
    // para recibir datos dialog
    @Optional() public dialogRef: MatDialogRef<DialogEventComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.FormGroupSend = this.formBuilder.group({
      FormCreate: new FormGroup({
        Title: new FormControl(null, Validators.required),
        Description: new FormControl(null, Validators.required),
        CreateRegisterDate: new FormControl(moment().format(
          'YYYY-MM-DD hh:mm:ss'
        )),
        CreateRegisterByUserId: new FormControl(
          '635AAA2F-27D6-4768-B8D5-85BE11715FC8',
          Validators.required
        ),
      }),
    });
    this.FormCreate = this.FormGroupSend.controls['FormCreate'] as FormGroup;
  }

  ngOnInit() {
  }
  sendForm() {
    this.FormGroupSend.markAllAsTouched();
    if (this.FormGroupSend.valid) {
      this.btnSubmit = false;
      //cargando ....
      Swal.fire('Enviando ...', 'Registro diligenciado correctamente', 'info');
      let objectForSubmit =
        this.FormGroupSend.controls['FormCreate'].getRawValue();
        objectForSubmit['Type'] = 1;
      this.globalService
        .postData(
          'http://172.168.8.80:7050/',
          'EventLog',
          objectForSubmit
        )
        .subscribe({
          next: (result) => {
            try {
              if (result.success) {
                this.FormGroupSend.disable();
                let data = result.entity;
                Swal.fire({
                  title: 'Exito',
                  icon: 'success',
                  html: ` Registrado correctamente
                    <h4><a href="/events/list">Clic aquí para consultar los últimos registros</a></h4>`,
                  confirmButtonColor: '#085092',
                  allowOutsideClick: false,
                  timer: 10000,
                  timerProgressBar: true,
                  willClose: () => {
                    window.location.reload();
                  },
                });
              }
            } catch (e) {
              console.log('error', e);
            }
          },
          error: (error) => {
            console.log('error:', error);
            let errorDisplay = error.error ? error.error : error;
            Swal.fire({
              title: 'Error',
              icon: 'error',
              html:
                ` <p>Por favor intente de nuevo.</p><h6>::` +
                errorDisplay +
                `::</h6>`,
              confirmButtonColor: '#085092',
              allowOutsideClick: false,
            });
            this.btnSubmit = true;
          },
        });
    }
  }
}
