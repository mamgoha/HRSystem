import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'departments',
   canActivate: [AuthGuard],
   loadChildren: () => import('./department/department.module')
    .then(mod => mod.DepartmentModule)},
  {path: 'employees',
   loadChildren: () => import('./employee/employee.module')
  .then(mod => mod.EmployeeModule)
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module')
    .then(mod => mod.AccountModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
