/** Single product image from catalog API (camelCase JSON). */
export interface ProductImage {
  uid: string;
  sortOrder: number;
  imageUrl: string;
  thumbnailImageUrl?: string;
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

/**
 * Public URL for the primary listing thumbnail, or null when none.
 * Falls back to full image URL when thumbnail is missing.
 */
export function primaryThumbnailImageUrl(product: Product): string | null {
  const images = product.images;
  if (!images?.length) {
    return null;
  }
  const primary = images.find((i) => i.sortOrder === 0) ?? images[0];
  const thumbUrl = primary?.thumbnailImageUrl?.trim();
  if (thumbUrl) {
    return thumbUrl;
  }
  const fullUrl = primary?.imageUrl?.trim();
  return fullUrl || null;
}
