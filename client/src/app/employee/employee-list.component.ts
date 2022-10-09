import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Employee } from '../_models/employee/employee';
import { EmployeeService } from '../_services/employee.service';
import { UpsertEmployeeComponent } from './upsert-employee/upsert-employee.component';

declare let alertify: any;

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  displayedColumns: string[] = ['empCode', 'empName', 'age', 'departmentName', 'action'];
  dataSource!: MatTableDataSource<any>;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  constructor(private employeeService: EmployeeService, public dialog: MatDialog) {
    alertify.set('notifier','position', 'top-right');
    alertify.defaults.theme.ok = "btn btn-primary";
    alertify.defaults.theme.cancel = "btn btn-danger";
  }

  ngOnInit(): void {
    this.getAllEmployees();
  }

  private getAllEmployees() {
    this.employeeService.getemployeees().subscribe({
      next: (res) => {
        debugger;
        this.dataSource = new MatTableDataSource<Employee>(res);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (e) =>{
        console.log(e);
      }
    });
  }

  openDialog() {
    this.dialog.open(UpsertEmployeeComponent, {
      width: '30%',
      disableClose: true,
      data: { parent: this, btnAction: 'Save'}
    });
  }

  editEmployee(row: any){
    this.dialog.open(UpsertEmployeeComponent, {
      width: '30%',
      disableClose: true,
      data: { parent: this, editData: row, btnAction: 'Update'}
    });
  }

  deleteEmployee(id: any){
    alertify.confirm('Delete', 'Do you really want to delete this employee?', 
    () => {
      this.employeeService.deleteemployee(id).subscribe({
        next: (res) =>{
          this.getAllEmployees();
          alertify.success('Employee deleted successfully.');
        },
        error: (e) => {
          alertify.error(e.error.title);
        }
      })
    },
    () => alertify.error('Cancel')).set('movable', false);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
