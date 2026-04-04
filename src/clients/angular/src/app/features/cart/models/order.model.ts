export interface OrderLine {
  productId: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Order {
  id: string;
  userId: string | null;
  cartId: string | null;
  status: string;
  totalAmount: number;
  lines: OrderLine[];
}

export interface PlaceOrderRequest {
  cartId: string;
  userId?: string | null;
}
