import './app.css';
import React from "react";
import CustomRoutes from './routes';
import {BrowserRouter} from 'react-router-dom';

function App() {
  return <BrowserRouter>  
    <CustomRoutes/>
  </BrowserRouter>
}

export default App;
