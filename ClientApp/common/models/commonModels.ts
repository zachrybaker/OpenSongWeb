
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

export module ErrorModels {

    export interface AuthError {
        code: string;
        message: string;
    }

}