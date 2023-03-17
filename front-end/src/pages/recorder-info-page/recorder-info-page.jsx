import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';

import { DataGrid } from '@mui/x-data-grid';
import { useCallback, useEffect, useState } from 'react';
import { CheckBox, Image } from '@mui/icons-material';
import { getDateTimeString } from '../../helpers/dateTimeHelper';
import "./recorder-info-page.css"
import Screenshot from '../../components/screenshot/screenshot';
import { downloadFile, get, patch } from '../../helpers/axiosHelper';
import { useNavigate, useParams } from 'react-router-dom';
import RecorderChart from '../../components/recorder-chart/recorder-chart';
import RecorderDetailsTile from '../../components/recorder-details-tile/recorder-details-tile';
import { Button, CardHeader, CircularProgress, LinearProgress, Switch } from '@mui/material';
import ScreenShareWindows from '../../components/screen-share-window/screen-share-windows';
import DeleteIcon from '@mui/icons-material/Delete';
import SummarizeIcon from '@mui/icons-material/Summarize';
import ScreenShareIcon from '@mui/icons-material/ScreenShare';
import { recorderApiUrl } from '../../constants';
import ConfirmationPopup from '../../components/confirmation-popup/confirmation-popup';

const RecorderInfoPage = () => {
    const [rows, setRows] = useState([]);
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [holderFullName, setHolderFullName] = useState("");
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [screenShare, setScreenShare] = useState(false);
    const [warnings, setWarnings] = useState(false);
    const [violations, setViolations] = useState(false);
    const [totalCount, setTotalCount] = useState(0);
    const [reportLoading, setReportLoading] = useState(false);
    const [controller, setController] = useState({
        page: 0,
        pageSize: 3
      });
    let params = useParams();

    useEffect(() => {
        async function getData() {
            try {
              var data = (await get(`${recorderApiUrl}/api/screenshots?page=${controller.page}&pageSize=${controller.pageSize}&recorderId=${params.id}&onlyWarnings=${warnings}&onlyViolations=${violations}`)).data;
              setTotalCount(data.total);
              setRows(data.data);
              setHolderFullName(data.holderFullName);
              setLoading(false);
            }
            catch {
              navigate("/not-found");
            }
        }

        setLoading(true);
        getData();
    }, [controller, warnings, violations]);

    
  const handlePageChange = (pagenumber) => {
    console.log(pagenumber);
    setController({
      ...controller,
      page: pagenumber
    });
  };
    
    const columns = [
        { 
            field: 'id', 
            headerName: 'ID',
            sortable: false,
            editable: false,
            headerClassName: 'grid-header',
            flex: 1
        },
        {
            field: 'timeCreated',
            headerName: 'Time created',
            editable: false,
            sortable: false,
            headerClassName: 'grid-header',
            flex: 1,
            valueGetter: (params) =>
                `${getDateTimeString(new Date(params.row.timeCreated.replace("T", " ") + " GMT"))}`,
        },
        {
            field: 'source',
            headerName: 'Image',
            editable: false,
            sortable: false,
            flex: 1,
            headerClassName: 'grid-header',
            renderCell: (row) => {
                return <div className="screenshot"><Screenshot url={row.value} id={row.id} recorderId={params.id} updateGrid={setController} page={controller.page} mark={row.row.mark}/></div>
          }
        }
    ];

    const downloadWeeklyReport = useCallback(async () => {
      setReportLoading(true);
      await downloadFile(`${recorderApiUrl}/api/reports/weeklyStat/${params.id}`);
      setReportLoading(false);
    });

    return <>
        <div className='main-info-wrapper'>
          <div className="holder-name">
              {holderFullName}
            </div>
            <div className='button-section'>
              <Button onClick={downloadWeeklyReport} variant="outlined" sx={{marginRight: 2}} 
                title="Download weekly report">{reportLoading && <div className='button-load'><CircularProgress size="1rem" /></div>}<SummarizeIcon/></Button>
              <Button onClick={() => setScreenShare(true)} variant="outlined" sx={{marginRight: 2}} title="Observe Screen Share"><ScreenShareIcon/></Button>
              <Button onClick={() => setDeletePopupOpened(true)} variant="contained" color="error"><DeleteIcon /></Button>
              {screenShare && <ScreenShareWindows recorderId={params.id} opened={screenShare} close={() => setScreenShare(false)}/>}
          </div>
        </div>
        <Box sx={{ height: 330, width: '100%', display: "flex", justifyContent: "space-between" }}>
            <Card sx={{ width: "49%"}}>
              <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Recorder activity today" sx={{background: "#1976D2", color: "#fff"}}/>
              <CardContent sx={{display: "flex", justifyContent: "center", alignItems: "center", flexDirection: "column", height: "100%"}}>
                  <RecorderDetailsTile recoredId={params.id}/>
              </CardContent>
            </Card>
            <Card sx={{ width: "49%"}}>
              <CardHeader titleTypographyProps={{variant:'subtitle1' }} title="Recorder times this week" sx={{background: "#1976D2", color: "#fff"}}/>
              <RecorderChart recoredId={params.id}/>
            </Card>
        </Box>
        <Box sx={{ height: 420, width: '100%', marginTop: 3}}>
        <DataGrid
            rows={rows}
            columns={columns}
            loading={loading}
            pageSize={controller.pageSize}
            rowHeight={100}
            rowCount={totalCount}
            disableColumnMenu
            pagination
            paginationMode="server"
            onPageChange={handlePageChange}
            disableSelectionOnClick
            experimentalFeatures={{ newEditingApi: true }}

            sx={{'& .alertState--2': {
                    bgcolor: "rgba(211, 47, 47, 0.2)",
                    '&:hover': {
                      bgcolor: "rgba(211, 47, 47, 0.2) !important",
                  },
                  },
                 '& .alertState--1': {
                    bgcolor: "rgba(240, 219, 108, 0.6)",
                    '&:hover': {
                      bgcolor: "rgba(240, 219, 108, 0.6)",
                  },
                  }}}
            getRowClassName={(params) => `alertState--${params.row.mark}`}
        />
        <div className="toggle-wrapper">
          <div>
              <Switch onChange={(e) => setWarnings(e.target.checked)} className={"switch"}/>
              <span>Only with warnings</span>
          </div>
          <div>
              <Switch onChange={(e) => setViolations(e.target.checked)} className={"switch"}/>
              <span>Only with violations</span>
          </div>
        </div>
        </Box>
        <ConfirmationPopup opened={deletePopupOpened} handleOk={async () => {
                    try {
                        await patch(`${recorderApiUrl}/api/recordings/activate/${params.id}?activeState=${false}`);
                        setDeletePopupOpened(false);
                        navigate("/recorders");
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message="Do you want to disable this recorder?"/>
    </>
}

export default RecorderInfoPage;