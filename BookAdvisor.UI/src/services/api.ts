import axios from "axios";


const API_URL = 'https://localhost:7220/api';       //backend port adresi

const api = axios.create({
    baseURL: API_URL,
    headers: { 'Content-Type': 'application/json', },
});

//Eğer localhost da token varsa istek atarken, istek içerisine token da ekle. 
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

export default api;
