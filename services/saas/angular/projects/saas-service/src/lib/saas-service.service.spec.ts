import { TestBed } from '@angular/core/testing';

import { SaasServiceService } from './saas-service.service';

describe('SaasServiceService', () => {
  let service: SaasServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SaasServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
