import { Navigate, useOutlet } from "react-router-dom";
import { useAuth } from "../../hooks/auth";
import {Link} from 'react';
import { useLocalStorage } from "../../hooks/useLocalStorage";

const ProtectedLayout = () => {
  const [tokens] = useLocalStorage("tokens", null);
  const outlet = useOutlet();

  if (!tokens) {
    return <Navigate to="/login" />;
  }

  return (
    <div>
      {outlet}
    </div>
  )
};

export default ProtectedLayout;