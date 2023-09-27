export interface IPagination {
  pageSize: number;
  page: number;
  sort: string;
  sortDirection: string;
  filter?: string;
  filterValue?: {
    key?: string;
    filterValues?: Array<any>;
  };
  pagesQuantity?: number;
  data?: any;
  totalRows?: number;
}
