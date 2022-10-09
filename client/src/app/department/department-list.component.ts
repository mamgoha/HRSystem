import { Component, OnInit, ViewChild } from '@angular/core';
import { DepartmentService } from '../_services/department.service';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { Department } from '../_models/department/Department';
import { MatDialog } from '@angular/material/dialog';
import { UpsertDepartmentComponent } from './upsert-department/upsert-department.component';

declare let alertify: any;

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.css']
})
export class DepartmentListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'departmentName', 'action'];
  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  constructor(private departmentService: DepartmentService, public dialog: MatDialog) {
    alertify.set('notifier','position', 'top-right');
    alertify.defaults.theme.ok = "btn btn-primary";
    alertify.defaults.theme.cancel = "btn btn-danger";
   }

  ngOnInit(): void {
    this.getAllDepartments();
  }

  openDialog() {
    this.dialog.open(UpsertDepartmentComponent, {
      width: '30%',
      disableClose: true,
      data: { parent: this, btnAction: 'Save'}
    });
  }

  editDepartment(row: any){
    this.dialog.open(UpsertDepartmentComponent, {
      width: '30%',
      disableClose: true,
      data: { parent: this, editData: row, btnAction: 'Update'}
    });
  }

  deleteDepartment(id: any){
    alertify.confirm('Delete', 'Do you really want to delete this department?', 
    () => {
      this.departmentService.deleteDepartment(id).subscribe({
        next: (res) =>{
          this.getAllDepartments();
          alertify.success('Department deleted successfully.');
        },
        error: (e) => {
          alertify.error(e.error.title);
        }
      })
    },
    () => alertify.error('Cancel')).set('movable', false);
  }
  
  private getAllDepartments() {
    this.departmentService.getDepartmentes().subscribe({
      next: (res) => {
        debugger;
        this.dataSource = new MatTableDataSource<Department>(res);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (e) =>{
        console.log(e);
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
