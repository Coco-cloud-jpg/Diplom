import { memo, useCallback } from "react";
import {useNavigate} from 'react-router-dom';
import '../../pages/entry-page/entry-page.css';
import "./not-found.css"

const NotFound = () => {
  const navigate = useNavigate();
  const navigateTo = useCallback((route) => () =>
    navigate(route)
  , [navigate]);

  return <section id="entry-page">
        <form>     
          <div className="notFoundWrapper">
            <span>4</span>
            <span>0</span>
            <span>4</span>
          </div>   
          <p>There is no content under path you specified.</p>
        <button onClick={navigateTo("/home")}>Go to home page</button></form>
        </section>
}

export default memo(NotFound);