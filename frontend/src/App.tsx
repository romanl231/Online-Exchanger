import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { useAxiosInterceptor } from "./hooks/useAxiosInterceptor";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import PrivateRoute from "./components/PrivateRoute";
import Register from "./pages/Register";

const App = () => {
  useAxiosInterceptor();

  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="register" element={<Register/>} />
          <Route
            path="/"
            element={
              <PrivateRoute>
                <Dashboard/>
              </PrivateRoute>
            }
          />
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;
