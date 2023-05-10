import { Button, Switch } from '@mui/material';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { useState } from 'react';
import { memo } from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { destroy, get, patch, put } from '../../helpers/axiosHelper';
import { getDateTimeString } from '../../helpers/dateTimeHelper';
import DeleteIcon from '@mui/icons-material/Delete';
import ConfirmationPopup from '../../components/confirmation-popup/confirmation-popup';
import { useCallback } from 'react';
import InfoIcon from '@mui/icons-material/Info';
import AutorenewIcon from '@mui/icons-material/Autorenew';
import { recorderApiUrl } from '../../constants';

const CompaniesPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [popupMessage, setPopupMessage] = useState("Do you want to disable this company?");
    const [activeState, setActiveState] = useState({id: "", isActive: false});
    const [includeDeleted, setIncludeDeleted] = useState(false);
    const [gridReload, setGridReload] = useState(false);
    const navigator = useNavigate();

    console.log("render")
    const renderDetailsButton = useCallback((params) => {
        const id = params.row.id;
        const isActive = params.row.isActive;

        return (
            <div className='buttonsWrapper'>
                <Button
                    color="primary"
                    size="small"
                    onClick={() => {
                        navigator(`/company-details/${id}`)
                    }}
                >
                    <InfoIcon />
                </Button> 
                <Button
                    color={isActive? "error": "success"}
                    size="small"
                    onClick={() => {
                        setActiveState({id: id, isActive: isActive});
                        setPopupMessage(`Do you want to ${isActive?"disable": "activate"} this company?`);
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
          field: 'name',
          headerName: 'Name',
          editable: false,
          flex: 1
        },
        {
            field: 'email',
            headerName: 'Email',
            editable: false,
            flex: 1
        },
        {
            field: 'dateCreated',
            headerName: 'Time created',
            editable: false,
            flex: 2,
            valueGetter: (params) =>
                `${getDateTimeString(new Date(params.row.dateCreated.replace("T", " ") + " GMT"))}`,
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
            let data = (await get(`${recorderApiUrl}/api/companies?includeDeleted=${includeDeleted}`)).data;
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
                        await patch(`${recorderApiUrl}/api/companies/${activeState.id}`);
                        setDeletePopupOpened(false);
                        setGridReload(!gridReload);
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message={popupMessage}/>
    </>
}

export default memo(CompaniesPage);