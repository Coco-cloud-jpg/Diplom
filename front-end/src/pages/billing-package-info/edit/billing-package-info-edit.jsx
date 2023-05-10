import { Button, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { memo, useEffect, useState } from "react";
import "./billing-package-info-edit.css"
import { get, post, put } from "../../../helpers/axiosHelper";
import { recorderApiUrl } from "../../../constants";

const BillingPackageInfoEdit = ({row, close, isEdit, reloadGrid}) => {
  const [formData, setFormData] = useState({...row});
  const [currencies, setCurrencies] = useState([]);
  
  const handleChange = (e) => {
    let { name, value } = e.target;
    console.log(name, value);
    if (name === undefined)
        name = "currencyShort";

    console.log(name, value);
    setFormData((prevData) => ({
        ...prevData,
        [name]: value,
      }));
  };
  
  const handleSubmit = async (e) => {
      e.preventDefault();

      if (formData["name"].length < 0 || 
          formData["maxUsersCount"] < 1 ||
          formData["maxRecordersCount"] < 1 ||
          formData["price"] < 1
          )
          return;//popup 
    
      try {
        const response = isEdit ? (await put(`${recorderApiUrl}/api/packages/${formData.id}`, formData)).data:(await post(`${recorderApiUrl}/api/packages`, formData)).data;
        console.log(response);
        close();
        reloadGrid();
      }
      catch (e) {
        console.log(e);
      }
    };
  
    useEffect(() => {    
        async function getCurrencies() {
            if (currencies[0])
                return;

            try {
                setCurrencies((await get(`${recorderApiUrl}/api/packages/currency`)).data);   
            } catch (error) {
                console.log(error);
            }
        }
        
        getCurrencies();
    }, [currencies]);


    return (
      <form onSubmit={handleSubmit} className="package-info-edit">
        <div>
            <label htmlFor="name">Name</label>
            <input
                type="text"
                id="name"
                name="name"
                value={formData.name || ""}
                onChange={handleChange}
            />
        </div>
        <div>
            <label htmlFor="maxUsersCount">Max Users Count</label>
            <input
                type="number"
                min="0"
                id="maxUsersCount"
                name="maxUsersCount"
                value={formData.maxUsersCount || ""}
                onChange={handleChange}
            />
        </div>
        <div>
            <label htmlFor="maxRecordersCount">Max Recorders Count</label>
            <input
                type="number"
                min="0"
                id="maxRecordersCount"
                name="maxRecordersCount"
                value={formData.maxRecordersCount || ""}
                onChange={handleChange}
            />
        </div>
        <div className="currencies-wrapper">
            <label htmlFor="price">Price</label>
            <input
                type="number"
                min="0"
                id="price"
                name="price"
                value={formData.price || ""}
                onChange={handleChange}
            />
            <FormControl sx={{ m: 1, maxWidth: 105, '&.Mui-focused': {color: "#0F2E2F"} }} size="small" required>
                <InputLabel id="currencies-label">Currency</InputLabel>
                <Select
                    labelId="currencies-label"
                    id="currencies"
                    label="Country"
                    value={formData["currencyShort"] ?? ""}
                    onChange={handleChange}
                >
                    {currencies.map(item => <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                        key={item.id} value={item.id}>{item.name}</MenuItem>)}
                    </Select>
            </FormControl>
        </div>
        <div className="save-button-wrapper">
            <Button type="submit" variant="contained" color="primary" className="button">
                Save
            </Button>
        </div>
      </form>
    );
  };
  
  export default memo(BillingPackageInfoEdit);