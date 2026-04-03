/** Product row from `GET /api/catalog/products` (camelCase JSON). */
export interface Product {
  id: string;
  categoryId: string;
  name: string;
  slug: string;
  sku: string;
  price: number;
}
