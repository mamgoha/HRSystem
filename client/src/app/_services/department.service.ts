import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DepartmentToAdd } from '../_models/department/DepartmentToAdd';
import { Department } from '../_models/department/Department';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private baseUrl: string = environment.baseUrl + 'api/';
  
  constructor(private http: HttpClient) { }

  public addDepartment(department: DepartmentToAdd) {
    return this.http.post(this.baseUrl + 'Department', department);
  }

  public updateDepartment(id: number, department: Department) {
    return this.http.put(this.baseUrl + 'Department/' + id, department);
  }

  public getDepartmentes(): Observable<Department[]> {
      return this.http.get<Department[]>(this.baseUrl + `Department`);
  }

  public deleteDepartment(id: number) {
      return this.http.delete(this.baseUrl + 'Department/' + id);
  }

  public getDepartmentById(id: number): Observable<Department> {
      return this.http.get<Department>(this.baseUrl + 'Department/' + id);
  }
}
