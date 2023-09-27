import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import * as moment from 'moment';
import 'moment-timezone';

// Configura la zona horaria por defecto para Moment.js
moment.locale('es'); // Opcional, establece el idioma en español si lo deseas
moment.tz.setDefault('America/Bogota'); // Reemplaza 'America/Bogota' por la zona horaria deseada

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
