/** Single product image from catalog API (camelCase JSON). */
export interface ProductImage {
  uid: string;
  sortOrder: number;
  imageUrl: string;
}

/** Product from catalog API (camelCase JSON). */
export interface Product {
  id: string;
  categoryId: number;
  name: string;
  slug: string;
  sku: string;
  price: number;
  shortDescription: string;
  description: string;
  categoryName: string | null;
  /** Ordered by server (sortOrder then uid). Omitted when empty on older payloads. */
  images?: ProductImage[];
}

/**
 * Public URL for the primary listing/detail image, or null when none.
 * Prefers `sortOrder === 0`, otherwise the first image in the list.
 */
export function primaryImageUrl(product: Product): string | null {
  const images = product.images;
  if (!images?.length) {
    return null;
  }
  const primary = images.find((i) => i.sortOrder === 0) ?? images[0];
  const url = primary?.imageUrl?.trim();
  return url || null;
}
