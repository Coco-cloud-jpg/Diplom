import { memo, useState } from "react";
import {useNavigate, useParams} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import {Snackbar,Alert,Switch} from '@mui/material';
import axios from 'axios';
import SuccessfullRegister from "../successful-register/successful-register";

const ResetPassword = () => {
  const navigate = useNavigate();
  const [notify, setNotify] = useState({open: false, message: ""});
  const [requesSubmit, setRequesSubmit] = useState(false);
  const [inputPassword, setInputPassword] = useState("");
  const [showPass, setShowPass] = useState(false);

  const togglePassword = () => {
    setShowPass(!showPass);
  };
  let params = useParams();

  const submit = async (e) => {
    e.preventDefault();

    try {
        if (inputPassword.length < 5) {
            setNotify({open:true, message: "Password should have at least 5 symbols!"});
            return;
        }
        
        var data = await axios.post('https://localhost:7063/api/register/password-reset', {password: inputPassword, requestId: params.requestId})
        console.log(data);
        setRequesSubmit(true);
    }
    catch (ex){
        setNotify({open:true, message: ex.response.data});
        setRequesSubmit(false);
    }
}

  return <section id="entry-page">
          <div className="formWrapper">
          {requesSubmit ? 
                  <SuccessfullRegister message={"Your password was successfully reset!"}></SuccessfullRegister>
                :
                <><Snackbar
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                open={notify.open}
                onClose={() => setNotify({open: false})}
                ><Alert severity="error">{notify.message}</Alert></Snackbar>
              <form>
                <h2>Reset Password</h2>
                <fieldset>
                  <ul>
                    <li>
                      <label htmlFor="email">New password</label>
                      <div>
                        <input 
                          value={inputPassword}
                          onChange={(e) => setInputPassword(e.target.value)}
                          type={showPass ? "text" : "password"}
                          id="password" 
                          required/>
                        <Switch defaultChecked onClick={togglePassword}/>
                      </div>
                    </li>
                  </ul>
                </fieldset>
                <button className="primaryButton" onClick={submit}>Send Reset Link</button>
              </form>
              <button className="backButton" type="button" onClick={() => navigate("/login")}>Go Back</button>
              </>}
          </div>
        </section>
}

export default memo(ResetPassword);