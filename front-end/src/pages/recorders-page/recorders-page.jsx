import { Button, Switch, TextField } from '@mui/material';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { useState } from 'react';
import { memo } from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { get, patch } from '../../helpers/axiosHelper';
import { getDateTimeString } from '../../helpers/dateTimeHelper';
import DeleteIcon from '@mui/icons-material/Delete';
import InfoIcon from '@mui/icons-material/Info';
import "./recorders-page.css"
import AutorenewIcon from '@mui/icons-material/Autorenew';
import ConfirmationPopup from '../../components/confirmation-popup/confirmation-popup';
import { useCallback } from 'react';
import AddUserPopup from '../../components/add-user-popup/add-user-popup';
import { recorderApiUrl } from '../../constants';

const RecordersPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [popupMessage, setPopupMessage] = useState("Do you want to disable this recorder?");
    const [activeState, setActiveState] = useState({id: "", isActive: false});
    const [includeDeleted, setIncludeDeleted] = useState(false);
    const [gridReload, setGridReload] = useState(false);
    const navigator = useNavigate();
    const [addUserPopup, setAddUserPopup] = useState(false);
    console.log("render")
    const renderDetailsButton = useCallback((params) => {
        const id = params.row.id;
        const isActive = params.row.isActive;

        return (
            <div className='buttonsWrapper'>
                {
                    params.row.isActive?<Button
                    color="primary"
                    size="small"
                    onClick={() => {
                        navigator(`/recorder-info/${id}`)
                    }}
                >
                    <InfoIcon />
                </Button> : <></>  
                }
                <Button
                    color={isActive? "error": "success"}
                    size="small"
                    onClick={() => {
                        setActiveState({id: id, isActive: isActive});
                        setPopupMessage(`Do you want to ${isActive?"disable": "activate"} this recorder?`);
                        setDeletePopupOpened(true);
                    }}
                >
                    {params.row.isActive? <DeleteIcon />: <AutorenewIcon />}
                </Button>
            </div>
        )
    }, [])

    const columns = [
        { 
            field: 'id', 
            headerName: 'ID',
            flex: 2
        },
        {
          field: 'holderName',
          headerName: 'First name',
          editable: false,
          flex: 1
        },
        {
          field: 'holderSurname',
          headerName: 'Last name',
          editable: false,
          flex: 1
        },
        {
            field: 'isActive',
            headerName: 'Is Active',
            editable: false,
            flex: 1
        },
        {
          field: 'screenshotsToday',
          headerName: 'Screenshots Today',
          type: 'number',
          editable: false,
          flex: 1
        },
        {
            field: 'screenshotsTotal',
            headerName: 'Screenshots Total',
            type: 'number',
            editable: false,
            flex: 1
        },
        {
            field: 'timeCreated',
            headerName: 'Time created',
            editable: false,
            flex: 2,
            valueGetter: (params) =>
                `${getDateTimeString(new Date(params.row.timeCreated.replace("T", " ") + " GMT"))}`,
        },
        {
            field: ' ',
            flex: 1,
            sortable: false,
            filterable: false,
            renderCell: renderDetailsButton,
            disableClickEventBubbling: true,
        }
      ];
    

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/recordings?includeDeleted=${includeDeleted}`)).data;
            setRows(data);
            console.log(data);

            setLoading(false);
        }
        setRows([]);
        setLoading(true);
        getData()
    }, [includeDeleted, setIncludeDeleted, gridReload])

    return <>
        <Box sx={{ 
            height: 780, 
            width: '100%',
            '& .isActive--false': {
                bgcolor: "rgba(211, 47, 47, 0.2)",
                '&:hover': {
                    bgcolor: "rgba(211, 47, 47, 0.2) !important",
                }
            },
        }}>
        <div className='recorders-page-panel'>
            <div><Switch onClick={(e) => {setIncludeDeleted(!includeDeleted)}}/>Include disabled</div>
            <Button onClick={() => setAddUserPopup(true)} variant="outlined" color="success">Add new recorder</Button>
            <AddUserPopup opened={addUserPopup} close={() => {
                setAddUserPopup(false);
                setGridReload(!gridReload);
            }
                }/>
        </div>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        pageSize={9}
        rowHeight={75}
        disableSelectionOnClick
        experimentalFeatures={{ newEditingApi: true }}
        getRowClassName={(params) => `isActive--${params.row.isActive}`}
      />
    </Box>
    <ConfirmationPopup opened={deletePopupOpened} handleOk={async () => {
                    try {
                        console.log("here");
                        await patch(`${recorderApiUrl}/api/recordings/activate/${activeState.id}?activeState=${!activeState.isActive}`);
                        setDeletePopupOpened(false);
                        setGridReload(!gridReload);
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message={popupMessage}/>
    </>
}

export default memo(RecordersPage);