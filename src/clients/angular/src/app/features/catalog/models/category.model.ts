/**
 * Matches API `CategoryResponse` (`id`, `name`, `slug`). Subcategories: extend when API exposes hierarchy.
 */
export interface Category {
  id: string;
  name: string;
  slug: string;
}
