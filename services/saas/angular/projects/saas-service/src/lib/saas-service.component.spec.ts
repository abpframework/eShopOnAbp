import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SaasServiceComponent } from './saas-service.component';

describe('SaasServiceComponent', () => {
  let component: SaasServiceComponent;
  let fixture: ComponentFixture<SaasServiceComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SaasServiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaasServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
