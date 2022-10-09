import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DepartmentService } from 'src/app/_services/department.service';

declare let alertify: any;

export interface DialogData {
  parent: any;
  editData: any;
  btnAction: string;
}

@Component({
  selector: 'app-new-department',
  templateUrl: './upsert-department.component.html',
  styleUrls: ['./upsert-department.component.css']
})
export class UpsertDepartmentComponent implements OnInit {
  departmentForm !:FormGroup;
  save: boolean = false;

  constructor(private formBuilder: FormBuilder, private departmentService: DepartmentService,
    private dialogRef: MatDialogRef<UpsertDepartmentComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  ngOnInit(): void {
    this.departmentForm = this.formBuilder.group({
      id: [''],
      departmentName : ['', Validators.required]
    });

    if(this.data.editData){
      debugger;
      this.departmentForm.controls['id'].setValue(this.data.editData.id);
      this.departmentForm.controls['departmentName'].setValue(this.data.editData.departmentName);
    }
  }

  addDepartment(){
    if(this.save) return;
    this.save = !this.save;

    if(!this.data.editData){
      if(this.departmentForm.valid){
        this.departmentService.addDepartment(this.departmentForm.value)
        .subscribe({
          next:(res) => {
            alertify.success(`${this.departmentForm.controls['departmentName'].value} has been added.`);
            this.departmentForm.reset();
            this.dialogRef.close();
            this.data.parent.getAllDepartments();
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
      this.updateDepartment();
    }
  }

  updateDepartment(){
    this.departmentService.updateDepartment(this.data.editData.id, this.departmentForm.value)
    .subscribe({
      next: (res) => {
        alertify.success(`${this.data.editData.departmentName} has been updated.`);
        this.departmentForm.reset();
        this.dialogRef.close();
        this.data.parent.getAllDepartments();
      },
      error: (e)=> {
        console.log(e);
        alertify.error(e.error.title);
      }
    })
  }

  
}
