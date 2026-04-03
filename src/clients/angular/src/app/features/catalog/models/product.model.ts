/** Product from catalog API (camelCase JSON). */
export interface Product {
  id: string;
  categoryId: string;
  name: string;
  slug: string;
  sku: string;
  price: number;
  shortDescription: string;
  description: string;
  categoryName: string | null;
}
