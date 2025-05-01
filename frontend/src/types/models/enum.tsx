export enum InventoryTransactionType {
    Inbound = 'Inbound',
    Outbound = 'Outbound',
    Adjustment = 'Adjustment'
}

export enum OrderStatus {
    Pending = 'Pending',
    Processing = 'Processing',
    Shipped = 'Shipped',
    Delivered = 'Delivered',
    Cancelled = 'Cancelled'
}

export enum VehicleType {
    FlatbedTrailer = 'FlatbedTrailer',
    LowbedTrailer = 'LowbedTrailer',
    Kipper = 'Kipper'
}

export enum VehicleStatus {
    Available = 'Available',
    InUse = 'InUse',
    Maintenance = 'Maintenance'
}

export enum ShipmentStatus {
    Preparing = 'Preparing',
    OutForDelivery = 'OutForDelivery',
    Delivered = 'Delivered'
}