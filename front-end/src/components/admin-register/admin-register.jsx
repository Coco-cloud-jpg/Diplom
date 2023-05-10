import { memo, useCallback ,useState, useEffect} from "react";
import {useNavigate, useParams} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import NotFound from "../not-found/not-found";
import { Switch, Snackbar, Alert} from "@mui/material";
import axios from "axios";
import SuccessfullRegister from '../successful-register/successful-register';

const AdminRegister = () => {
  const navigate = useNavigate();
  let [email, setEmail] = useState("");
  const [showPass, setShowPass] = useState(false);
  const [inputFirstName, setInputFirstName] = useState("");
  const [inputLastName, setInputLastName] = useState("");
  const [inputPassword, setInputPassword] = useState("");
  const [registrationSubmit, setRegistrationSubmit] = useState(false);
  let params = useParams();
  const [notify, setNotify] = useState({open: false, message: ""});

  useEffect(() => {
    async function getEmail() {
        console.log(params);
        try {
            setEmail((await axios.get(`https://localhost:7063/api/register/email/${params.companyId}`)).data);   
        } catch (error) {
            navigate("/not-found")
        }
    }
    
    getEmail();
}, [email]);

  const togglePassword = () => {
    setShowPass(!showPass);
  };

  if (!/^[0-9a-f]{8}-[0-9a-f]{4}-[0-5][0-9a-f]{3}-[089ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(params.companyId))
    return <NotFound />;


    const submit = async (e) => {
        e.preventDefault();

        try {
            console.log(inputFirstName, inputLastName, inputPassword);
            if (inputFirstName.length < 3 || inputLastName.length < 3 || inputPassword.length < 5) {
                setNotify({open: true, message: "Fill all the fields!"});
                return;
            }

            const data = await axios.post('https://localhost:7063/api/register/admin', 
                {
                    firstName: inputFirstName, 
                    lastName: inputLastName, 
                    email: email, 
                    password: inputPassword,
                    companyId: params.companyId,
                })
            console.log(data);
            setRegistrationSubmit(true);
        }
        catch{
            setNotify({open:true, message: "We can't process your request right now."});
            setRegistrationSubmit(false);
        }
    }

    const handleChange = (event, callback) => {
        callback(event.target.value);
    };

  return <section id="entry-page">
        {registrationSubmit ? 
            <SuccessfullRegister><p>You can now login to the system!</p></SuccessfullRegister>:
            <>
                <Snackbar
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                    open={notify.open}
                    onClose={() => setNotify({open: false})}
                    ><Alert severity="error">{notify.message}</Alert></Snackbar>
                    <form>
                <h2>Admin Registration</h2>
                <fieldset>
                    <ul>
                    <li>
                        <label htmlFor="firstName">First name</label>
                        <input type="text" id="firstName" value={inputFirstName} onChange={(e) => handleChange(e, setInputFirstName)}  required minLength="3"/>
                    </li>
                    <li>
                        <label htmlFor="lastName">Last Name</label>
                        <input type="text" id="lastName" value={inputLastName} onChange={(e) => handleChange(e, setInputLastName)}  required minLength="3"/>
                    </li>
                    <li>
                        <label htmlFor="email">Email</label>
                        <input type="email" id="email" required disabled value={email} onChange={(e) => handleChange(e, setEmail)}/>
                    </li>
                    <li>
                        <label htmlFor="password">Password</label>
                        <div>
                            <input type={showPass ? "text" : "password"} id="password" value={inputPassword} onChange={(e) => handleChange(e, setInputPassword)}  required minLength="5"/>
                            <Switch defaultChecked onClick={togglePassword}/>
                        </div>
                    </li>
                    </ul>
                </fieldset>
                <button className="primaryButton" onClick={submit}>Create</button>
            </form>
            </>
        }  
        </section>
}

export default memo(AdminRegister);