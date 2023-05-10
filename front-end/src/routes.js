import React, { useEffect, useState } from "react";
import { Navigate, Routes, Route, Router } from "react-router-dom";
import HomePage from "./pages/home-page/home-page"
import Login from "./components/login/login";
import Register from "./components/register/register";
import ForgotPassword from "./components/forgot-password/forgot-password";
import AdminRegister from "./components/admin-register/admin-register";
import LoginLayout from "./layouts/login-layout/login-layout";
import ProtectedLayout from "./layouts/protected-layout/protected-layout";
import NotFound from "./components/not-found/not-found";
import ResetPassword from "./components/reset-password/reset-password";
import RecordersPage from "./pages/recorders-page/recorders-page";
import RecorderInfoPage from "./pages/recorder-info-page/recorder-info-page";
import AlertRulesPage from "./pages/alert-rules-page/alert-rules.page";
import WarningsPage from "./pages/warnings-page/warnings-page";
import ReportsPage from "./pages/reports-page/reports-page";
import { get } from "./helpers/axiosHelper";
import HomeAdminPage from "./pages/home-admin-page/home-admin-page";
import CompaniesPage from "./pages/companies-page/companies-page";
import CompanyDetailsPage from "./pages/company-details-page/company-details-page";
import BillingPage from "./pages/billing-page/billing-page";
import UsersPage from "./pages/users-page/users-page";
import UserRegisterConfirmPage from "./pages/user-register-confirm-page/user-register-confirm-page";

const routesToRoute = {
    "/home": <Route path="home" element={<HomePage />} />,
    "/recorders": <Route path="recorders" element={<RecordersPage />}/>,
    "/recorder-info/:id": <Route path="recorder-info/:id" element={<RecorderInfoPage />} />,
    "/alerts": <Route path="alerts" element={<AlertRulesPage />} />,
    "/warnings": <Route path="warnings" element={<WarningsPage />} />,
    "/reports": <Route path="reports" element={<ReportsPage />} />
}

const CustomRoutes = () => {
   return   <Routes>
                <Route element={<LoginLayout />}>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/forgot-password" element={<ForgotPassword />} />
                </Route>
                <Route path="/password-reset/:requestId" element={<ResetPassword />} />
                <Route path="/admin-register/:companyId" element={<AdminRegister />} />
                <Route path="/user-register/:companyId/:userId" element={<UserRegisterConfirmPage />} />
                <Route path="/" element={<ProtectedLayout />}>
                    <Route path="home" element={<HomePage />} />
                    <Route path="home-admin" element={<HomeAdminPage />} />
                    <Route path="recorders" element={<RecordersPage />}/>
                    <Route path="billing" element={<BillingPage />}/>
                    <Route path="recorder-info/:id" element={<RecorderInfoPage />} />
                    <Route path="company-details/:id" element={<CompanyDetailsPage />} />
                    <Route path="alerts" element={<AlertRulesPage />} />
                    <Route path="companies" element={<CompaniesPage />} />
                    <Route path="warnings" element={<WarningsPage />} />
                    <Route path="reports" element={<ReportsPage />} />
                    <Route path="users" element={<UsersPage />} />
                </Route>
                <Route path="*" element={<NotFound />}/>
            </Routes>;
}

export default CustomRoutes