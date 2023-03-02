import { memo, useCallback, useState } from "react";
import {useNavigate} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import {Snackbar,Alert, LinearProgress} from '@mui/material';
import axios from 'axios';
import SuccessfullRegister from "../successful-register/successful-register";

const ForgotPassword = () => {
  const navigate = useNavigate();
  const [notify, setNotify] = useState({open: false, message: ""});
  const [requesSubmit, setRequesSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [inputEmail, setInputEmail] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
        if (inputEmail === "") {
            setNotify({open:true, message: "Provide email to send reset link to!"});
            return;
        }
        
        var data = await axios.post('https://localhost:7063/api/register/password-reset-request', {email: inputEmail})
        console.log(data);
        setLoading(false);
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
            <SuccessfullRegister><p>Mail with password reset link was sent to your mailbox!</p></SuccessfullRegister>
              :
              <><Snackbar
              anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
              open={notify.open}
              onClose={() => setNotify({open: false})}
              ><Alert severity="error">{notify.message}</Alert></Snackbar>
            <form>
              <h2>Reset Password Request</h2>
              <fieldset>
                <ul>
                  <li>
                    <label htmlFor="email">Email:</label>
                    <input type="email" id="email" value={inputEmail} onChange={(e) => setInputEmail(e.target.value)} required/>
                  </li>
                </ul>
              </fieldset>
              <button className="primaryButton" onClick={submit}>Send Reset Link</button>
            </form>
            <div className='progress'>
                {loading && <LinearProgress />}
              </div>
            <button className="backButton" type="button" onClick={() => navigate("/login")}>Go Back</button>
            </>}
            </div>
        </section>
}

export default memo(ForgotPassword);