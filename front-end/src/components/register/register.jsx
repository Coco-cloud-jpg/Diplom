import { memo, useCallback, useEffect, useState } from "react";
import {useNavigate} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import {FormControl, MenuItem, InputLabel, Select, Snackbar,Alert} from '@mui/material';
import axios from 'axios';
import "./register.css";
import SuccessfullRegister from "../successful-register/successful-register";
import "../../styles/general.css"

const Register = ({setView}) => {
    const navigate = useNavigate();
    const navigateTo = useCallback((route) => () =>
      navigate(route)
    , [navigate]);
    const [countries, setCountries] = useState([]);
    const [inputCountry, setInputCountry] = useState("");
    const [inputName, setInputName] = useState("");
    const [inputEmail, setInputEmail] = useState("");
    const [notify, setNotify] = useState({open: false, message: ""});
    const [registrationSubmit, setRegistrationSubmit] = useState(false);

    useEffect(() => {    
        async function getCountries() {
            if (countries[0])
                return;

            try {
                setCountries((await axios.get(`https://localhost:7063/api/register/countries`)).data);   
            } catch (error) {
                console.log(error);
            }
        }
        
        getCountries();
    }, [countries]);

    const handleChange = (event, callback) => {
        callback(event.target.value);
    };

    const submit = async (e) => {
        e.preventDefault();

        try {
            if (inputName.length < 5 || inputEmail.length < 5 || inputCountry === '') {
                setNotify({open: true, message: "Fill all the fields!"});
                return;
            }
            let emailExists = (await axios.get(`https://localhost:7063/api/register/email/exists/${inputEmail}`)).data;
    
            if (emailExists) {
                setNotify({open:emailExists, message: "This email already exists in system!"});
                return;
            }
            
            let data = await axios.post('https://localhost:7063/api/register/company', {name: inputName, email: inputEmail, countryId: inputCountry})
            console.log(data);
            setRegistrationSubmit(true);
        }
        catch{
            setNotify({open:true, message: "We can't process your request right now."});
            setRegistrationSubmit(false);
        }
    }

  return <section id="entry-page">
            <div className="formWrapper">
            {registrationSubmit ? 
                    <SuccessfullRegister><p>Follow steps send to your mailbox to finish registration!</p></SuccessfullRegister>
                :
                <><Snackbar
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
                open={notify.open}
                onClose={() => setNotify({open: false})}
                ><Alert severity="error">{notify.message}</Alert></Snackbar>
                    <form>
                        <h2>Company Registration</h2>
                        <fieldset>
                            <ul>
                            <li>
                                <label htmlFor="name">Name*</label>
                                <input type="text" id="name" value={inputName} onChange={(e) => handleChange(e, setInputName)} minLength={5} required/>
                            </li>
                            <li>
                                <label htmlFor="email">Email*</label>
                                <input type="email" id="email" value={inputEmail} onChange={(e) => handleChange(e, setInputEmail)} minLength={5} required />
                            </li>
                            <FormControl sx={{ m: 1, minWidth: 315, '&.Mui-focused': {color: "#0F2E2F"} }} size="small" required>
                                <InputLabel id="demo-simple-select-label">Country</InputLabel>
                                <Select
                                    labelId="demo-simple-select-label"
                                    id="demo-simple-select"
                                    label="Country"
                                    value={inputCountry}
                                    onChange={(e) =>  handleChange(e, setInputCountry)}
                                >
                                    {countries.map(item => <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                                    key={item.id} value={item.id}>{item.name}</MenuItem>)}
                                </Select>
                            </FormControl>
                            </ul>
                        </fieldset>
                        <button className="primaryButton" onClick={submit}>Create</button>
                    </form>
                    
                    <button className="backButton" type="button" onClick={ navigateTo("/login")}>Go back</button>
                    </>
                }
            </div>
        </section>
}

export default memo(Register);