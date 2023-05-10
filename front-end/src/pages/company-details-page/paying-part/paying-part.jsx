import { Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { memo, useEffect, useState } from "react";
import { get, post } from "../../../helpers/axiosHelper";
import { recorderApiUrl } from "../../../constants";
import "./paying-part.css";
import { getDateTimeString } from "../../../helpers/dateTimeHelper";

const PayingPart = ({companyId}) => {
    const [transactions, setTransactions] = useState({});
    const [currencies, setCurrencies] = useState([]);
    const [formData, setFormData] = useState({});
    const [loading, setLoading] = useState(true);
    const [reloadTransactions, setReloadTransactions] = useState(true);

    const handleChange = (e) => {
        let { name, value } = e.target;
        console.log(name, value);
        if (name === undefined)
            name = "currency";
    
        console.log(name, value);
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
          }));
    };

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/transactions/${companyId}`)).data;
            setTransactions(data);
            setLoading(false);

            if (!currencies[0])
                setCurrencies((await get(`${recorderApiUrl}/api/packages/currency`)).data);   
        }
        
        setLoading(true);
        getData()
    }, [reloadTransactions])

    const submit = async (e) => {
        e.preventDefault();
        try {
            formData.companyId = companyId;
            await post(`${recorderApiUrl}/api/transactions/`, formData);
            setReloadTransactions(!reloadTransactions);
        }
        catch (e) {
            console.log(e);
        }
    };

    return <div className="transactions">
            {loading ? 
                <CircularProgress />:
                <div className="transaction-menu-wrapper">
                    <div className="add-transaction">
                        <form className="sum-wrapper">
                            <input
                                type="number"
                                min="0"
                                id="sum"
                                name="sum"
                                value={formData.sum || ""}
                                onChange={handleChange}
                            />
                            <FormControl sx={{ m: 1, width: 105, '&.Mui-focused': {color: "#0F2E2F"} }} size="small" required>
                                <InputLabel id="currencies-label">Currency</InputLabel>
                                <Select
                                    labelId="currencies-label"
                                    id="currencies"
                                    label="Country"
                                    value={formData["currency"] ?? ""}
                                    onChange={handleChange}
                                >
                                    {currencies.map(item => <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                                        key={item.id} value={item.id}>{item.name}</MenuItem>)}
                                    </Select>
                            </FormControl>
                            <button variant="contained" onClick={submit}>
                                Submit
                            </button>
                        </form>
                    </div>
                    <div className="transaction-wrapper">
                        {transactions.map(item => 
                                <div className="list-item" key={item.id}>
                                    <p><span className="sum">{(item.sum).toFixed(2)} {(item.currency === 0 ? "$": item.currency === 1? "€": "₴")}</span>-<span>{getDateTimeString(new Date(item.paymentDate.replace("T", " ") + " GMT"))}</span></p>
                                </div>
                        )}
                    </div>
                </div>
            }
        </div>;
}

export default memo(PayingPart);