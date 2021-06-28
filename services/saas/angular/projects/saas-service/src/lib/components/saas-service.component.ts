import { Component, OnInit } from '@angular/core';
import { SaasServiceService } from '../services/saas-service.service';

@Component({
  selector: 'lib-saas-service',
  template: ` <p>saas-service works!</p> `,
  styles: [],
})
export class SaasServiceComponent implements OnInit {
  constructor(private service: SaasServiceService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
