import { memo, useEffect, useState } from "react";
import { get } from "../../../helpers/axiosHelper";
import { recorderApiUrl } from "../../../constants";
import { CircularProgress } from "@mui/material";
import "./billing-package-info-companies.css";
import { useNavigate } from "react-router-dom";

const BillingPackageCompanies = ({id}) => {
  const [companiesList, setCompaniesList] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {    
    async function getCompanies() {
        if (companiesList[0])
            return;

        try {
            setCompaniesList((await get(`${recorderApiUrl}/api/packages/${id}/companies`)).data);   
        } catch (error) {
            console.log(error);
        }

        setLoading(false);
    }
    setLoading(true);
    getCompanies();
  }, []);
  //add redirect to companies details
  return <div className="content-wrapper">
    {loading ? <CircularProgress />: <div className="companies-wrapper">{companiesList.length > 0 ? companiesList.map(item =>
        <div className="list-item" key={item.companyId} onClick={() => navigate(`/company-details/${item.companyId}`)}>
            <h4>{item.companyName}<span title="Packages count">{item.count}</span></h4>
            <p>{item.companyId}</p>
        </div>
    ): <div className="no-content-info">No companies attached</div>}</div>}
  </div>
};
  
export default memo(BillingPackageCompanies);