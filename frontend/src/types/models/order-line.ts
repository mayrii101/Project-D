import { Order } from './order';
import { Product } from './product';

export interface OrderLine {
    id: number;
    orderId: number;
    order: Order;
    productId: number;
    product: Product;
    quantity: number;
    lineTotal: number; // Calculated field
    isDeleted: boolean;
}