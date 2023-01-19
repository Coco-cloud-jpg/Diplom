import { Navigate, useOutlet } from "react-router-dom";
import { useAuth } from "../../hooks/auth";
import { useLocalStorage } from "../../hooks/useLocalStorage";

const LoginLayout = () => {
  const [tokens] = useLocalStorage("tokens", null);
  const outlet = useOutlet();

  if (tokens) {
    return <Navigate to="/home" />;
  }

  return (
    <div>
      {outlet}
    </div>
  )
};

export default LoginLayout;