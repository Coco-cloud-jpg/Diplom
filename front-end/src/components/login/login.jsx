import axios from "axios";
import { memo, useCallback, useState } from "react";
import {useNavigate} from 'react-router-dom';
import { setAuthToken } from "../../helpers/setToken";
import { useLocalStorage } from "../../hooks/useLocalStorage";
import '../../pages/entry-page/entry-page.css';
import {FaDoorOpen} from 'react-icons/fa';

const Login = () => {
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();
  const navigateTo = useCallback((route) => () =>
    navigate(route)
  , [navigate]);

  const handleSubmit = useCallback((e) => {
    async function submit() {
      try {
        console.log(e)
        //reqres registered sample user
        const loginPayload = {
          email: email,
          password: password
        }
  
        var response = await axios.post("https://localhost:7063/api/token", loginPayload);
        console.log(response.data);
        //set JWT token to local
        setTokens(response.data);
        navigate('/home');
      } 
      catch (e){
        console.log(e);
      }  
    }
    e.preventDefault();
    submit();
  }, [navigate, email, password]);

  return <section id="entry-page">
        <form>
          <h2>Login</h2>
          <fieldset>
            <ul>
              <li>
                <label htmlFor="username">Email:</label>
                <input 
                  value={email}
                  onChange={(e) => {console.log("qweqwe");setEmail(e.target.value)}}
                  type="text" 
                  id="username" 
                  required/>
              </li>
              <li>
                <label htmlFor="password">Password:</label>
                <input 
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  type="password"
                  id="password" 
                  required/>
              </li>
              <li>
                <i/>
                <a onClick={ navigateTo("/forgot-password")}>Forgot Password?</a>
              </li>
            </ul>
          </fieldset>
          <button onClick={handleSubmit} className="primaryButton">Submit</button>
          <button type="button" onClick={ navigateTo("/register")}>Create a Company</button>
        </form>
        </section>
}

export default memo(Login);