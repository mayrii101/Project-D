import { Product } from './product';
import { Warehouse } from './warehouse';

export interface Inventory {
    id: number;
    productId: number;
    product: Product;
    warehouseId: number;
    warehouse: Warehouse;
    quantityOnHand: number;
    lastUpdated: Date;
    isDeleted: boolean;
}