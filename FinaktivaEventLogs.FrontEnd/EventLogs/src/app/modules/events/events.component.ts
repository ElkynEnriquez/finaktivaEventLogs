import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogEventComponent } from './dialog-event/dialog-event.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  // Variable encargada de almacenar los datos que se envian desde el modal
  public dataReceived: any;
  constructor(
    public dialog: MatDialog,
  ) { }

  ngOnInit() {
  }
  createEvent() {
    const dialogRef = this.dialog.open(DialogEventComponent, {
      maxWidth: '90vw',
      maxHeight: '90vh',
      width: '850px',
      data: {
        input: null,
        output: this.dataReceived,
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === "OK"){
        Swal.fire({
          title: 'Eso es todo!',
          icon: 'success',
          html: ` <p>Tú registro a finalizado con éxito.</p>
            <h4><a href="/events/list">Clic aquí para ver registros</a></h4>`,
          confirmButtonColor: '#085092',
          allowOutsideClick: false,
        });
      }
    });
  }
}
