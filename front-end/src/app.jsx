import './app.css';
import React from "react";
import CustomRoutes from './routes';
import {BrowserRouter} from 'react-router-dom';
import { useState, useEffect } from "react";
import { Backdrop,LinearProgress,  makeStyles,
  createStyles,} from '@mui/material';

function App() {
  return <>
  <BrowserRouter> 
    <CustomRoutes/>
  </BrowserRouter>
  </>
}

export default App;
