import api from './api';


//request modeli
export interface LoginRequest {
    email: string;
    password: string;
}

///response modeli
export interface LoginResponse {
    token: string;
}

//login servis
export const authService = {
    Login: async (data: LoginRequest) => {
        //POST api/Auth/login
        const response = await api.post<LoginResponse>('/Auth/login', data);
        return response.data;
    },

    logout: () => {
        localStorage.removeItem('token');
    }
};


