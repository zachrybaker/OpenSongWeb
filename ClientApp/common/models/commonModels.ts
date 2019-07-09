
export module CommonModels {

    export interface PagingParameter {
        total?: number;
        pageSize?: number;
        currentIndex?: number;
        description?: string; 
    }

    export interface PaginatedList extends PagingParameter
    {
        items: Array<any>;
    }
}