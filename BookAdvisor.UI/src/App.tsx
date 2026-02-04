import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage } from "./pages/LoginPage";
import { HomePage } from './pages/HomePage';


function App() {
  return (
    //BrowserRouter, uygulamada url değişirse takip etmeye yarar.
    <BrowserRouter>
      <Routes >                                                     {/*İçindeki Route'lardan sadece birini  seçer  */}
        <Route path="/login" element={<LoginPage />} />             {/* URL "/login" ise LoginPage'i göster */}
        <Route path='/HomePage' element={<HomePage />} />       {/* URL "/" (ana sayfa) ise, kullanıcıyı "/login"e yönlendir (Redirect) */}
        <Route path='/' element={<Navigate to="/login" />} />       {/* URL "/" (ana sayfa) ise, kullanıcıyı "/login"e yönlendir (Redirect) */}
      </Routes>

    </BrowserRouter>
  )
}

export default App
