import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  UnitsResponse, 
  ComparisonRequest, 
  ComparisonResponse, 
  ConvertRequest, 
  QuantityDTO,
  ArithmeticRequest,
  DivideResponse,
  MultiplyResponse
} from '../models';

@Injectable({
  providedIn: 'root'
})
export class MeasurementService {
  private readonly BASE_URL = '/api';

  constructor(private http: HttpClient) {}

  getUnits(): Observable<UnitsResponse> {
    return this.http.get<UnitsResponse>(`${this.BASE_URL}/Units`);
  }

  compare(request: ComparisonRequest): Observable<ComparisonResponse> {
    return this.http.post<ComparisonResponse>(`${this.BASE_URL}/Quantity/compare`, request);
  }

  convert(request: ConvertRequest): Observable<QuantityDTO> {
    return this.http.post<QuantityDTO>(`${this.BASE_URL}/Quantity/convert`, request);
  }

  add(request: ArithmeticRequest): Observable<QuantityDTO> {
    return this.http.post<QuantityDTO>(`${this.BASE_URL}/Quantity/add`, request);
  }

  subtract(request: ArithmeticRequest): Observable<QuantityDTO> {
    return this.http.post<QuantityDTO>(`${this.BASE_URL}/Quantity/subtract`, request);
  }

  divide(request: ArithmeticRequest): Observable<DivideResponse> {
    return this.http.post<DivideResponse>(`${this.BASE_URL}/Quantity/divide`, request);
  }

  multiply(request: ArithmeticRequest): Observable<MultiplyResponse> {
    return this.http.post<MultiplyResponse>(`${this.BASE_URL}/Quantity/multiply`, request);
  }
}
