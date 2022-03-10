import { TestBed } from '@angular/core/testing';

import { OrderingService } from './ordering.service';

describe('OrderingService', () => {
  let service: OrderingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrderingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
