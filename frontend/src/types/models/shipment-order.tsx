import { Shipment } from './shipment';
import { Order } from './order';

export interface ShipmentOrder {
    shipmentId: number;
    shipment: Shipment;
    orderId: number;
    order: Order;
}