import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Department } from 'src/app/_models/department/Department';
import { DepartmentService } from 'src/app/_services/department.service';
import { EmployeeService } from 'src/app/_services/employee.service';

declare let alertify: any;

export interface DialogData {
  parent: any;
  editData: any;
  btnAction: string;
}


@Component({
  selector: 'app-upsert-employee',
  templateUrl: './upsert-employee.component.html',
  styleUrls: ['./upsert-employee.component.css']
})
export class UpsertEmployeeComponent implements OnInit {
  departments!: Department[];
  employeeForm !:FormGroup;
  save: boolean = false;

  constructor(private formBuilder: FormBuilder, private employeeService: EmployeeService, private departmentService: DepartmentService,
    private dialogRef: MatDialogRef<UpsertEmployeeComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  ngOnInit(): void {
    this.departmentService.getDepartmentes().subscribe({
      next:(res) => this.departments = res,
      error: (e) => console.log(e)
    });

    this.employeeForm = this.formBuilder.group({
      id: [''],
      empCode : ['', Validators.required],
      empName : ['', Validators.required],
      dateOfBirth : [''],
      address : [''],
      mobile : [''],
      salary : [''],
      departmentId : ['', Validators.required],
    });

    if(this.data.editData){
      this.employeeService.getemployeeById(this.data.editData.id).subscribe({
        next: (res) => {
          debugger;
          this.employeeForm.controls['id'].setValue(res.id);
          this.employeeForm.controls['empCode'].setValue(res.empCode);
          this.employeeForm.controls['empName'].setValue(res.empName);
          this.employeeForm.controls['dateOfBirth'].setValue(res.dateOfBirth);
          this.employeeForm.controls['address'].setValue(res.address);
          this.employeeForm.controls['mobile'].setValue(res.mobile);
          this.employeeForm.controls['salary'].setValue(res.salary);
          this.employeeForm.controls['departmentId'].setValue(res.departmentId);
        },
        error: (e) => console.log(e)
      });

      
    }
  }

  addEmployee(){
    if(this.save) return;
    this.save = !this.save;

    if(!this.data.editData){
      if(this.employeeForm.valid){
        this.employeeService.addemployee(this.employeeForm.value)
        .subscribe({
          next:(res) => {
            alertify.success(`Employee has been added.`);
            this.employeeForm.reset();
            this.dialogRef.close();
            this.data.parent.getAllEmployees();
          },
          error: (e) => {
            alertify.error(e.error.title);
            console.log(e)
          },
          complete: () =>{
            this.save = !this.save;
          }
        });
      }
    } else {
      this.updateEmployee();
    }
  }

  updateEmployee(){
    console.log(this.employeeForm.value);
    debugger;
    this.employeeService.updateemployee(this.data.editData.id, this.employeeForm.value)
    .subscribe({
      next: (res) => {
        alertify.success(`Employee has been updated.`);
        this.employeeForm.reset();
        this.dialogRef.close();
        this.data.parent.getAllEmployees();
      },
      error: (e)=> {
        console.log(e);
        alertify.error(e.error.title);
      }
    })
  }

  keyPressNumbers(event: any) {
    var charCode = (event.which) ? event.which : event.keyCode;
    // Only Numbers 0-9
    if ((charCode < 48 || charCode > 57)) {
      event.preventDefault();
      return false;
    } else {
      return true;
    }
  }

}
