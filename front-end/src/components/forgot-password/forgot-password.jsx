import { memo, useCallback } from "react";
import {useNavigate} from 'react-router-dom';
import { FaArrowLeft } from 'react-icons/fa';
import '../../pages/entry-page/entry-page.css';

const ForgotPassword = () => {
  const navigate = useNavigate();
  const navigateTo = useCallback((route) => () =>
    navigate(route)
  , [navigate]);

  return <section id="entry-page">
        <form>
          <h2>Reset Password</h2>
          <fieldset>
            <ul>
              <li>
                <label htmlFor="email">Email:</label>
                <input type="email" id="email" required/>
              </li>
            </ul>
          </fieldset>
          <button className="primaryButton">Send Reset Link</button>
          <button className="backButton" type="button" onClick={navigateTo("/login")}>
              <FaArrowLeft className="arrow"/><span>Go Back</span>
          </button>
        </form>
        </section>
}

export default memo(ForgotPassword);