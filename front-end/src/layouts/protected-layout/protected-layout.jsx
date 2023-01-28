import { Navigate, useOutlet } from "react-router-dom";
import { useAuth } from "../../hooks/auth";
import {Link, useEffect} from 'react';
import { useLocalStorage } from "../../hooks/useLocalStorage";
import Sidenav from "../../components/sidenav/sidenav";
import './protected-layout.css';
import HomePage from "../../pages/home-page/home-page";
import {refreshTokens} from '../../helpers/axiosHelper';

const ProtectedLayout = () => {
  const [tokens] = useLocalStorage("tokens", null);
  const outlet = useOutlet();

  //refreshTokens();

  if (!tokens) {//restores
    return <Navigate to="/login" />;
  }

  return (
    <Sidenav>
    <div className="content">
      {outlet ?? <HomePage />}
    </div>
    </Sidenav>
  )
};

export default ProtectedLayout;