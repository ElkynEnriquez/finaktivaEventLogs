import { NgModule } from '@angular/core';
import { SharedComponent } from './shared.component';
import { NumberToCurrencyPipe } from './pipes/numberToCurrency.pipe';
import { SafeUrlPipe } from './pipes/safeUrl.pipe';
import { DirectivesModule } from './directives/directives.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { UppercaseDirective } from './directives/uppercase.directive';
import { JwtInterceptorInterceptor } from './jwt-interceptor.interceptor';



@NgModule({
  declarations: [
    SharedComponent,
    NumberToCurrencyPipe,
    SafeUrlPipe,
  ],
  imports: [
    DirectivesModule,
    HttpClientModule,
  ],
  exports: [
    SharedComponent,
    UppercaseDirective,
    HttpClientModule,
    NumberToCurrencyPipe,
    SafeUrlPipe,
    DirectivesModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorInterceptor,
      multi: true,
    },
  ]
})
export class SharedModule { }
