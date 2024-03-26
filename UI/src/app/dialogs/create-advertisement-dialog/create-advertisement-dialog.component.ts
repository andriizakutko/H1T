import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-advertisement-dialog',
  templateUrl: './create-advertisement-dialog.component.html',
  styleUrls: ['./create-advertisement-dialog.component.css']
})
export class CreateAdvertisementDialogComponent {
  firstFormGroup: FormGroup;
  
  secondFormGroup = this._formBuilder.group({
    title: ['', Validators.required],
    subTitle: ['', Validators.required],
    description: ['', Validators.required],
    price: [0, Validators.required],
    country: ['', Validators.required],
    city: ['', Validators.required],
    address: ['', Validators.required],
  });
  thirdFormGroup = this._formBuilder.group({
    engineCapacity: [0, Validators.required],
    serialNumber: ['', Validators.required],
    fuelConsumption: [0, Validators.required],
    mileage: [0, Validators.required],
    manufactureCountry: ['', Validators.required],
    manufactureDate: ['', Validators.required],
    isElectric: [false, Validators.required],
    isUsed: [false, Validators.required]
  });
  fourthFormGroup = this._formBuilder.group({
    images: [[], Validators.required],
  });
  isLinear = true;

  constructor(private _formBuilder: FormBuilder) {
    this.firstFormGroup = this._formBuilder.group({
      transportType: { value: '', disabled: false },
      transportMake: { value: '', disabled: true },
      transportModel: { value: '', disabled: true },
      transportBody: { value: '', disabled: true },
      transportBodyType: { value: '', disabled: true },
    });

    this.firstFormGroup.get('transportType')!.setValidators(Validators.required);
    this.firstFormGroup.get('transportMake')!.setValidators(Validators.required);
    this.firstFormGroup.get('transportModel')!.setValidators(Validators.required);
    this.firstFormGroup.get('transportBody')!.setValidators(Validators.required);
    this.firstFormGroup.get('transportBodyType')!.setValidators(Validators.required);

    this.firstFormGroup.updateValueAndValidity();
  }

  ngOnInit(): void {
    this.fetchTransportTypes();
  }

  resetFirstFormGroup() {
    this.firstFormGroup.get('transportType')!.setValue('');
    this.firstFormGroup.get('transportMake')!.setValue('');
    this.firstFormGroup.get('transportModel')!.setValue('');
    this.firstFormGroup.get('transportBody')!.setValue('');
    this.firstFormGroup.get('transportBodyType')!.setValue('');

    this.firstFormGroup.get('transportType')!.enable();
    this.firstFormGroup.get('transportMake')!.disable();
    this.firstFormGroup.get('transportModel')!.disable();
    this.firstFormGroup.get('transportBody')!.disable();
    this.firstFormGroup.get('transportBodyType')!.disable();
  }

  selectTransportTypeProcess() {
    if(this.firstFormGroup.get('transportBodyType')!.value !== '' 
      || this.firstFormGroup.get('transportMake')!.value !== '') 
    {
      this.firstFormGroup.get('transportBodyType')!.setValue('');
      this.firstFormGroup.get('transportMake')!.setValue('');
      this.firstFormGroup.get('transportModel')!.setValue('');
      this.firstFormGroup.get('transportModel')!.disable();
    }
    this.fetchTransportBodyTypes();
    this.fetchTransportMakes();
    this.firstFormGroup.get('transportBodyType')!.enable();
    this.firstFormGroup.get('transportMake')!.enable();
  }

  selectTransportMakeProcess() {
    if(this.firstFormGroup.get('transportModel')!.value !== '')
    {
      this.firstFormGroup.get('transportModel')!.setValue('');
    }
    this.fetchTransportModels();
    this.firstFormGroup.get('transportModel')!.enable();
  }

  fetchTransportTypes() {
    console.log('fetch data about transport types')
  }

  fetchTransportMakes() {
    console.log('fetch data about transport makes')
  }

  fetchTransportModels() {
    console.log('fetch data about transport models')
  }

  fetchTransportBodyTypes() {
    console.log('fetch data about transport body types')
  }
}
