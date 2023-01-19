import { memo, useCallback } from "react";
import {useNavigate} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import {FaArrowLeft} from 'react-icons/fa';

const Register = ({setView}) => {
    const navigate = useNavigate();
    const navigateTo = useCallback((route) => () =>
      navigate(route)
    , [navigate]);

  return <section id="entry-page">
        <form>
            <h2>Company Registration</h2>
            <fieldset>
                <ul>
                <li>
                    <label htmlFor="username">Username:</label>
                    <input type="text" id="username" required/>
                </li>
                <li>
                    <label htmlFor="email">Email:</label>
                    <input type="email" id="email" required/>
                </li>
                <li>
                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" required/>
                </li>
                </ul>
            </fieldset>
            <button className="primaryButton">Create</button>
            <button className="backButton" type="button" onClick={ navigateTo("/login")}><FaArrowLeft className="arrow"/><span>Back to login</span></button>
        </form>
        </section>
}

export default memo(Register);