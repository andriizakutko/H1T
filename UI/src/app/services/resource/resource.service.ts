import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Response } from 'src/app/models/Response';

@Injectable({
  providedIn: 'root'
})
export class ResourceService {
  private apiUrl = 'https://localhost:7110/api/resource/';

  constructor(private http: HttpClient) { }

  getTransportTypes() {
    return this.http.get<Response>(this.apiUrl + 'get-transport-types');
  }

  getTransportBodyTypes(id: string) {
    return this.http.get<Response>(this.apiUrl + `get-transport-body-types/transportTypeId=${id}`);
  }

  getTransportMakes(id: string) {
    return this.http.get<Response>(this.apiUrl + `get-transport-makes/transportTypeId=${id}`);
  }

  getTransportModels(id: string) {
    return this.http.get<Response>(this.apiUrl + `get-transport-models/transportMakeId=${id}`);
  }
}
