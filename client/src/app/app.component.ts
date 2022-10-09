import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  title = 'client';
  constructor(private accountService: AccountService){

  }
  
  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser() {
    debugger;
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token!).subscribe({
      next: (v:any) => console.log('loaded user'),
      error: (e:any) => console.log(e)
    });
  }
}
