export class PagedList<T> {

    public innerList: T[];
    public pageIndex: number;
    public pageSize : number;
    public totalCount : number;
    public totalPages : number;
    public hasPreviousPage  : boolean;
    public hasNextPage  : boolean;
}