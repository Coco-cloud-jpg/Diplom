import { createContext, useContext, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { useLocalStorage } from "./useLocalStorage";
const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const navigate = useNavigate();

  // call this function when you want to authenticate the user
  const login = async (data) => {
    setTokens(data);
    navigate("/home");
  };

  // call this function to sign out logged in user
  const logout = () => {
    tokens(null);
    navigate("/login", { replace: true });
  };

  const value = useMemo(
    () => ({    
      tokens,
      login,
      logout
    }),
    [tokens]
  );
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  return useContext(AuthContext);
};