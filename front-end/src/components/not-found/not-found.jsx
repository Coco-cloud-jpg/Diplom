import { memo, useCallback } from "react";
import {useNavigate} from 'react-router-dom';
import { FaArrowLeft } from 'react-icons/fa';
import '../../pages/entry-page/entry-page.css';

const NotFound = () => {
  const navigate = useNavigate();
  const navigateTo = useCallback((route) => () =>
    navigate(route)
  , [navigate]);

  return <section id="entry-page">
        Not found
        <button onClick={navigateTo("/home")}>Go to home page</button>
        </section>
}

export default memo(NotFound);