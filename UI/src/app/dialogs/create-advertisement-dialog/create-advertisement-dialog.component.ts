import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Value } from 'src/app/models/Value';
import { ResourceService } from 'src/app/services/resource/resource.service';

@Component({
  selector: 'app-create-advertisement-dialog',
  templateUrl: './create-advertisement-dialog.component.html',
  styleUrls: ['./create-advertisement-dialog.component.css']
})
export class CreateAdvertisementDialogComponent {
  firstFormGroup: FormGroup;
  transportTypes: Value[] = [];
  transportBodyTypes: Value[] = [];
  transportMakes: Value[] = [];
  transportModels: Value[] = [];
  previousTypeName: string = '';
  previousMakeName: string = '';
   
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

  constructor(private _formBuilder: FormBuilder, private resourceService: ResourceService) {
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

    this.transportBodyTypes = [];
    this.transportMakes = [];
    this.transportModels = [];

    this.previousTypeName = '';
    this.previousMakeName = '';
  }

  selectTransportTypeProcess(id: string, typeName: string) {
    if(typeName !== this.previousTypeName
      || this.previousTypeName === '') {
      this.previousTypeName = typeName;
      if(this.firstFormGroup.get('transportBodyType')!.value !== '' 
      || this.firstFormGroup.get('transportMake')!.value !== '') 
      {
        this.transportBodyTypes = [];
        this.transportMakes = [];
        this.transportModels = [];
        this.firstFormGroup.get('transportBodyType')!.setValue('');
        this.firstFormGroup.get('transportMake')!.setValue('');
        this.firstFormGroup.get('transportModel')!.setValue('');
        this.firstFormGroup.get('transportModel')!.disable();
      }
      this.fetchTransportBodyTypes(id);
      this.fetchTransportMakes(id);
      this.firstFormGroup.get('transportBodyType')!.enable();
      this.firstFormGroup.get('transportMake')!.enable();
    }
  }

  selectTransportMakeProcess(id: string, makeName: string) {
    if(makeName !== this.previousMakeName
      || this.previousMakeName === '')
    {
      this.previousMakeName = makeName;
      if(this.firstFormGroup.get('transportModel')!.value !== '')
      {
        this.transportModels = [];
        this.firstFormGroup.get('transportModel')!.setValue('');
      }
      this.fetchTransportModels(id);
      this.firstFormGroup.get('transportModel')!.enable();
    }
  }

  fetchTransportTypes() {
    this.resourceService.getTransportTypes().subscribe(response => {
      if(response.statusCode === 200) {
        this.transportTypes = Object.values(response.data);
      }
    });
  }

  fetchTransportMakes(id: string) {
    this.resourceService.getTransportMakes(id).subscribe(response => {
      if(response.statusCode === 200) {
        this.transportMakes = Object.values(response.data);
      }
    })
  }

  fetchTransportModels(id: string) {
    this.resourceService.getTransportModels(id).subscribe(response => {
      if(response.statusCode === 200) {
        this.transportModels = Object.values(response.data);
      }
    })
  }

  fetchTransportBodyTypes(id: string) {
    this.resourceService.getTransportBodyTypes(id).subscribe(response => {
      if(response.statusCode === 200) {
        this.transportBodyTypes = Object.values(response.data);
      }
    })
  }
}
