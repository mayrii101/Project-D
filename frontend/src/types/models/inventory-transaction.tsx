import { Product } from './product';
import { Employee } from './employee';
import { InventoryTransactionType } from './enum';

export interface InventoryTransaction {
    id: number;
    productId: number;
    product: Product;
    quantity: number;
    type: InventoryTransactionType;
    timestamp: Date;
    employeeId: number;
    employee: Employee;
    sourceOrDestination: string;
    isDeleted: boolean;
}