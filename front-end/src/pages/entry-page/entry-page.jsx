import { Component, memo, useCallback, useMemo, useState } from "react"
import './entry-page.css'
import Login from '../../components/login/login';
import Register from '../../components/register/register';
import ForgotPassword from '../../components/forgot-password/forgot-password';

const EntryPage = () => {
  const [currentView, setCurrentView] = useState("logIn");

  const setView = useCallback((value) => {
    setCurrentView(value);
  }, [setCurrentView])

  console.log("sdadqwq");
  const view = useMemo(() => {
    switch(currentView) {
      case "signUp":
        return <Register setView={setView}/>
      case "logIn":
        return <Login setView={setView}/>
      case "PWReset":
        return <ForgotPassword setView={setView}/>
      default:
        return <div>Error</div>
    }
  }, [currentView, setCurrentView]);

  return (
      <section id="entry-page">
        {view}
      </section>
  )
}

export default memo(EntryPage);