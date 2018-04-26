export class PagedList<T> {

    innerList: T[];
    pageIndex: number;
    pageSize : number;
    totalCount : number;
    totalPages : number;
    hasPreviousPage  : boolean;
    hasNextPage  : boolean;
}