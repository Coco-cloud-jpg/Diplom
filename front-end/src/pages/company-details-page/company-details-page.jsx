import { Box, Button, Card, CardContent, CardHeader, CircularProgress } from "@mui/material"
import { memo, useEffect, useState } from "react"
import ApartmentIcon from '@mui/icons-material/Apartment';
import "./company-details-page.css";
import DeleteIcon from '@mui/icons-material/Delete';
import { get, patch } from "../../helpers/axiosHelper";
import { recorderApiUrl } from "../../constants";
import { useNavigate, useParams } from "react-router-dom";
import ConfirmationPopup from "../../components/confirmation-popup/confirmation-popup";
import CompanysPackagesInfo from "./packages-info/companys-packages-info";
import { getDateTimeString } from "../../helpers/dateTimeHelper";
import PayingPart from "./paying-part/paying-part";

const CompanyDetailsPage = () => {
    const [companyInfo, setCompanyInfo] = useState({});
    const [companyInfoLoading, setCompanyInfoLoading] = useState(true);
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [finalTimeExpired, setFinalTimeExpired] = useState(false);
    const navigator = useNavigate();
    let params = useParams();

    useEffect(() => {
      async function getCompanyInfo() {
        let data = (await get(`${recorderApiUrl}/api/companies/${params.id}`)).data;
        data.timeToPay = new Date(data.timeToPay.replace("T", " ") + " GMT");
        setFinalTimeExpired(data.timeToPay < new Date())
        setCompanyInfo(data);
        setCompanyInfoLoading(false);
      }
      setCompanyInfoLoading(true)
      getCompanyInfo();
    }, []);

    return <>
    <div className="company-details-wrapper">
      <div className="left-side">
        <Card>
          <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Company info" sx={{background: "#1976D2", color: "#fff"}}/>
          <div className="company-details-info">
            <div className="icon-wrapper"><ApartmentIcon style={{color: "#1976D2"}}/></div>
            {companyInfoLoading ? <CircularProgress />:<div className="text-info">
              <div><span>Id:</span> {companyInfo.id}</div>
              <div><span>Name:</span> {companyInfo.name}</div>
              <div><span>Email:</span> {companyInfo.email}</div>
              <div><span>Date Created:</span> {companyInfo.dateCreated}</div>
              <div><span>Country:</span> {companyInfo.country}</div>
              <div className={finalTimeExpired? "bill-date-expired": ""}><span>Time To Pay:</span> {getDateTimeString(companyInfo.timeToPay)}</div>
            </div>}
            <div>
              <Button color="error" onClick={() => {setDeletePopupOpened(true)}}>
                <DeleteIcon />
              </Button>
            </div>
          </div>
        </Card>
        <Card>
          <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Transactions" sx={{background: "#1976D2", color: "#fff"}}/>
          <PayingPart companyId={params.id}/>
        </Card>
      </div>
      <div className="right-side">
        <Card sx={{ width: "100%"}}>
          <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Billing" sx={{background: "#1976D2", color: "#fff"}}/>
          <CompanysPackagesInfo companyId={params.id}/>
        </Card>
      </div>
    </div>
    <ConfirmationPopup opened={deletePopupOpened} handleOk={async () => {
                    try {
                        await patch(`${recorderApiUrl}/api/companies/${params.id}`);
                        setDeletePopupOpened(false);
                        navigator("/companies");
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message="Do you want to disable this company?"/>
    </>
    /*return <Box sx={{ height: "86vh", width: '100%', display: "flex", justifyContent: "space-between" }}>
    <Card sx={{ width: "49%"}}>
      <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Recorder times this week" sx={{background: "#1976D2", color: "#fff"}}/>
      <CardContent sx={{display: "flex", justifyContent: "center", alignItems: "center", flexDirection: "column", height: "100%"}}>
      </CardContent>
    </Card>
</Box>*/
}

export default memo(CompanyDetailsPage)