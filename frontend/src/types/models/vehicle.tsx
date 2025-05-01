import { VehicleType, VehicleStatus } from './enum';

export interface Vehicle {
    id: number;
    licensePlate: string;
    capacityKg: number;
    type: VehicleType;
    status: VehicleStatus;
    isDeleted: boolean;
}