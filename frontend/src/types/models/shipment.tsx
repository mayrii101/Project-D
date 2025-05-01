import { Vehicle } from './vehicle';
import { Employee } from './employee';
import { ShipmentStatus } from './enum';
import { ShipmentOrder } from './shipment-order';
import { Order } from './order';

export interface Shipment {
    id: number;
    vehicleId: number;
    vehicle: Vehicle;
    driverId: number;
    driver: Employee;
    status: ShipmentStatus;
    departureDate: Date;
    expectedDeliveryDate?: Date | null;
    actualDeliveryDate?: Date | null;
    isDeleted: boolean;
    shipmentOrders: ShipmentOrder[];
    orders: Order[]; // Derived from shipmentOrders
}