import { memo } from "react";
import { useNavigate } from "react-router-dom";
import '../../pages/entry-page/entry-page.css';
import "./successful-register.css";
import { useLocalStorage } from "../../hooks/useLocalStorage";

const SuccessfulRegister = ({children, message = "Your request has been submitted!"}) => {
  const navigateTo = useNavigate();
  const [tokens, setTokens] = useLocalStorage("tokens", null); 

  return <div className="formWrapper successRegister">
          <div>{message}</div>
          {children}
          <button className="backButton" type="button" onClick={ () => {setTokens(null); navigateTo("/login")}}>Back to login</button>
        </div>;
}

export default memo(SuccessfulRegister);