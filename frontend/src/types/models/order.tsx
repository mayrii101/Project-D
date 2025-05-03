import { Customer } from './customer';
import { OrderLine } from './order-line';
import { ShipmentOrder } from './shipment-order';
import { OrderStatus } from './enum';

export interface Order {
    id: number;
    customerId: number;
    customer: Customer;
    productLines: OrderLine[];
    totalWeight: number; // Calculated field
    status: OrderStatus;
    orderDate: Date;
    deliveryAddress: string;
    expectedDeliveryDate: Date;
    actualDeliveryDate?: Date | null;
    isDeleted: boolean;
    shipmentOrders: ShipmentOrder[];
}