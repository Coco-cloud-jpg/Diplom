import axios from "axios";
import { memo, useCallback, useState } from "react";
import {useNavigate} from 'react-router-dom';
import { setAuthToken } from "../../helpers/setToken";
import { useLocalStorage } from "../../hooks/useLocalStorage";
import '../../pages/entry-page/entry-page.css';
import {Switch, Snackbar, Alert, LinearProgress} from "@mui/material";
import jwt from 'jwt-decode';
import "../../styles/general.css";

const Login = () => {
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPass, setShowPass] = useState(false);
  const [loading, setLoading] = useState(false);
  const [notify, setNotify] = useState({open: false, message: ""});

  const togglePassword = () => {
    setShowPass(!showPass);
  };

  const navigate = useNavigate();
  const navigateTo = useCallback((route) => () =>
    navigate(route)
  , [navigate]);

  const handleSubmit = useCallback((e) => {
    async function submit() {
      try {
        var response = await axios.post("https://localhost:7063/api/token", {
          email: email,
          password: password
        });
        
        setTokens(response.data);
        console.log(jwt(response.data.access));
        setLoading(false);
        navigate('/home');
      } 
      catch (e){
        setLoading(false);
        setNotify({open: true, message: e.response.data});
        console.log(e);
      }  
    }
    e.preventDefault();
    setLoading(true);
    submit();
  }, [navigate, email, password]);

  return <section id="entry-page">
        <div className="formWrapper">
        <Snackbar
          anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
          open={notify.open}
          onClose={() => setNotify({open: false})}
          ><Alert severity="error">{notify.message}</Alert></Snackbar>
        <form>
          <h2>Login</h2>
          <fieldset>
            <ul>
              <li>
                <label htmlFor="username">Email</label>
                <input 
                  value={email}
                  onChange={(e) => {console.log("qweqwe");setEmail(e.target.value)}}
                  type="text" 
                  id="username" 
                  required/>
              </li>
              <li>
                <label htmlFor="password">Password</label>
                <div>
                  <input 
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    type={showPass ? "text" : "password"}
                    id="password" 
                    required/>
                  <Switch defaultChecked onClick={togglePassword} className={"switch"}/>
                </div>
              </li>
              <li>
                <i/>
                <a onClick={ navigateTo("/forgot-password")}>Forgot Password?</a>
              </li>
            </ul>
          </fieldset>
          <button onClick={handleSubmit} className="primaryButton">Submit</button>
        </form>
        <div className='progress'>
             {loading && <LinearProgress />}
        </div>
        <button className="createCompany" type="button" onClick={ navigateTo("/register")}>Create a Company</button>
        </div>
        </section>
}

export default memo(Login);