import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { GlobalService } from 'projects/shared/src/lib/services/global.service';

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
  ) {
    this.FormGroupSend = this.formBuilder.group({
      FormCreate: new FormGroup({
        Title: new FormControl(null, Validators.required),
        Description: new FormControl(null, Validators.required),
        CreateRegisterDate: new FormControl(null),
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

}
