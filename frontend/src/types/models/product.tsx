export interface Product {
    id: number;
    productName: string;
    sku: string;
    weightKg: number;
    material: string;
    batchNumber: number;
    price: number;
    category: string;
    expirationDate?: Date | null;
    isDeleted: boolean;
}