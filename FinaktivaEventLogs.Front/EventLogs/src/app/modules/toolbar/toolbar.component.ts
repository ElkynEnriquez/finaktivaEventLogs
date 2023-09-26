import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {

  constructor( ) { }

  ngOnInit(): void { }
  /**
   * Método encargado de mostrar un mensaje de bienvenida con ayuda
   */
   helpMe() {
    Swal.fire({
      title: '¡Bienvenido(a) a LiliSoft!',
      html: `<p>En nuestro sistema, podrás navegar en el menú del lado izquierdo de la pantalla (icono de 3 lineas de la barra superior para móviles)</p>
            <p>El menú se adaptará según el rol que desempeñe actualmente y los permisos que se le han aprobado.
              <h4>* Si cree que se le deben activar más funcionalidades o permisos puede hacer su solicitud al correo <a href="mailto:dev@finaktiva.com" target="_blank">dev@finaktiva.com</a></h4>
            </p>`,
      imageUrl: 'https://finaktiva.com/assets/images/miniatura-redes-sociales.jpg',
      imageWidth: 300,
      imageAlt: 'Logo',
      width: 800,
      background: 'rgba(255,255,255,0.95)',
      backdrop: `
        rgba(0,48,73,0.5)
        left top
        no-repeat
      `,
    });
  }

}
