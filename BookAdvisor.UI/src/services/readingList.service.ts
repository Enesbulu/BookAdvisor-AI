import api from './api';

//backend den gelecek veri için dto
export interface ReadingListDto {
    id: string;
    name: string;
    bookCount: number;
}

//backend istek servisi
export const ReadingListService = {
    //GET /api/reading-lists
    getAll: async () => {
        const response = await api.get<ReadingListDto[]>('/reading-lists');
        return response;
    },
};


