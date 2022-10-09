import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee } from '../_models/employee/employee';
import { EmployeeToAdd } from '../_models/employee/employeeToAdd';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private baseUrl: string = environment.baseUrl + 'api/';

  constructor(private http: HttpClient) { }

  public addemployee(employee: EmployeeToAdd) {
    return this.http.post(this.baseUrl + 'Employee', employee);
  }

  public updateemployee(id: number, employee: Employee) {
    debugger;
    return this.http.put(this.baseUrl + 'Employee/' + id, employee);
  }

  public getemployeees(): Observable<Employee[]> {
      return this.http.get<Employee[]>(this.baseUrl + `Employee`);
  }

  public deleteemployee(id: number) {
      return this.http.delete(this.baseUrl + 'Employee/' + id);
  }

  public getemployeeById(id: number): Observable<Employee> {
      return this.http.get<Employee>(this.baseUrl + 'Employee/' + id);
  }
}
