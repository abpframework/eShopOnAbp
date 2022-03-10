import type { GetMyOrdersInput, GetOrdersInput, OrderCreateDto, OrderDto, UpdateOrderDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  apiName = 'Ordering';

  create = (input: OrderCreateDto) =>
    this.restService.request<any, OrderDto>({
      method: 'POST',
      url: '/api/ordering/order',
      body: input,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, OrderDto>({
      method: 'GET',
      url: `/api/ordering/order/${id}`,
    },
    { apiName: this.apiName });

  getByOrderNo = (orderNo: number) =>
    this.restService.request<any, OrderDto>({
      method: 'GET',
      url: '/api/ordering/order/by-order-no',
      params: { orderNo },
    },
    { apiName: this.apiName });

  getMyOrders = (input: GetMyOrdersInput) =>
    this.restService.request<any, OrderDto[]>({
      method: 'GET',
      url: '/api/ordering/order/my-orders',
      params: { filter: input.filter },
    },
    { apiName: this.apiName });

  getOrders = (input: GetOrdersInput) =>
    this.restService.request<any, OrderDto[]>({
      method: 'GET',
      url: '/api/ordering/order/orders',
      params: { filter: input.filter },
    },
    { apiName: this.apiName });

  update = (id: string, input: UpdateOrderDto) =>
    this.restService.request<any, OrderDto>({
      method: 'PUT',
      url: `/api/ordering/order/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
