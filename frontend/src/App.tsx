import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import PrivateRoute from "./components/PrivateRoute";
import Register from "./pages/Register";
import { ToastContainer } from 'react-toastify';
import HomePage from "./pages/Homepage";
import Layout from "./pages/Layout";
import CreateListingPage from "./pages/CreateListingPage";

const App = () => {

  return (
    <AuthProvider>
      <Router>
        <ToastContainer
          position="top-right"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
        />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="register" element={<Register/>} />

          <Route element={<Layout />} >
          <Route path="/" element={<HomePage />} />
          
            <Route path="/dashboard" element={
              <PrivateRoute> 
                <Dashboard />
              </PrivateRoute> 
              } 
              />

              <Route 
              path="/listing/create" 
              element={
              <PrivateRoute>
                <CreateListingPage/>
              </PrivateRoute>} />
          </Route>
          
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;
