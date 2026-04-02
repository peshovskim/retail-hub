/**
 * Matches API `CategoryResponse` (`id`, `name`, `slug`).
 */
export interface Category {
  id: string;
  name: string;
  slug: string;
}

/** Matches API `CategoryMenuNodeResponse` (camelCase JSON). */
export interface CategoryMenuNode {
  id: string;
  name: string;
  slug: string;
  children: CategoryMenuNode[];
}
