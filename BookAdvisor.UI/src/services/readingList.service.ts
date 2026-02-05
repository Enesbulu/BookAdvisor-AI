import api from './api';

//backend den GET-ReadingLists gelecek veri için dto
export interface ReadingListDto {
    id: string;
    name: string;
    bookCount: number;
}

//Create ReadingLsit için kullanılan Dto yapısı
export interface CreateReadingListRequest {
    name: string;
}

///Backend istek servisi
export const ReadingListService = {
    ///Bütün ReadingList leri listeleme metodu
    //GET /api/reading-lists
    getAll: async () => {
        const response = await api.get<ReadingListDto[]>('/reading-lists');
        return response;
    },

    ///ReadingList Oluşturma metodu
    //POST /api/reading-lists
    create: async (data: CreateReadingListRequest) => {
        const response = await api.post<string>('/reading-lists', data);
        return response.data;
    }
};


