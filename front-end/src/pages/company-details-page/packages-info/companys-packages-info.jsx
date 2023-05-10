import { Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { memo, useEffect, useState } from "react";
import { get } from "../../../helpers/axiosHelper";
import { recorderApiUrl } from "../../../constants";
import "./company-packages-info.css";

const CompanysPackagesInfo = ({companyId}) => {
    const [billingInfo, setBillingInfo] = useState({});
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/companies/${companyId}/packages`)).data;
            setBillingInfo(data);

            setLoading(false);
        }
        
        setLoading(true);
        getData()
    }, [])

    return <div className="company-billing">
            {loading ? 
                <CircularProgress />:
                <div className="billing-info">
                    <div className="prices" title="Money to monthly pay">
                        <div>{billingInfo.monthlyDollarsCharge.toFixed(2)} $</div>
                        <div>{billingInfo.monthlyEurosCharge.toFixed(2)} €</div>
                        <div>{billingInfo.monthlyUAHCharge.toFixed(2)} ₴</div>
                    </div>
                    <div className="usages">
                        <h3>Resources usages</h3>
                        <div className="users" title={`${billingInfo.usersCount}/${billingInfo.maxUsersCount} users`}>
                            <h4>Users usage</h4>
                            <div className="users-count">
                                <div style={{width: `${billingInfo.usersCount/billingInfo.maxUsersCount * 100}%`}}></div>
                            </div>
                        </div>
                        <div className="recorders" title={`${billingInfo.recordersCount}/${billingInfo.maxRecordersCount} recorders`}>
                            <h4>Recorders usage</h4>
                            <div className="recorders-count">
                                <div style={{width: `${billingInfo.recordersCount/billingInfo.maxRecordersCount * 100}%`}}></div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <h3>Packages</h3>
                        <div className="packages-list">
                            {billingInfo.packages.map(item => 
                                <div className="list-item" key={item.name}>
                                    <h4>{item.name}</h4>
                                    <p>Maximum users count: {item.maxUsersCount}</p>
                                    <p>Maximum recorders count: {item.maxRecordersCount}</p>
                                    <h5>Count: {item.total}</h5>
                                    <h5>Price per unit: {(item.price/item.total).toFixed(2)} {(item.currency === 0 ? "$": item.currency === 1? "€": "₴")}</h5>
                                    <h5>Total price: {item.price.toFixed(2)} {(item.currency === 0 ? "$": item.currency === 1? "€": "₴")}</h5>
                                </div>
                                )}
                        </div>
                    </div>
                </div>
            }
        </div>;
}

export default memo(CompanysPackagesInfo);