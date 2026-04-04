export interface CartLine {
  productId: string;
  name: string;
  sku: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Cart {
  id: string;
  anonymousKey: string | null;
  lines: CartLine[];
  subtotal: number;
  itemCount: number;
}

export interface CartSession {
  cartId: string;
  anonymousKey: string;
}
