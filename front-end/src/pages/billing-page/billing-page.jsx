import { Button, Switch } from '@mui/material';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { useState } from 'react';
import { memo } from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { destroy, get, patch, put } from '../../helpers/axiosHelper';
import DeleteIcon from '@mui/icons-material/Delete';
import ConfirmationPopup from '../../components/confirmation-popup/confirmation-popup';
import { useCallback } from 'react';
import InfoIcon from '@mui/icons-material/Info';
import "./billing-page.css";
import { recorderApiUrl } from '../../constants';
import BillingPackageInfo from '../billing-package-info/billing-package-info';
import BillingPackageAdd from '../billing-package-info/add/billing-package-add';

const BillingPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedRow, setSelectedRow] = useState({});
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [popupMessage, setPopupMessage] = useState("Do you want to delete this package?");
    const [activeState, setActiveState] = useState({id: ""});
    const [gridReload, setGridReload] = useState(false);
    const navigator = useNavigate();

    const [infoDialogOpen, setInfoDialogOpen] = useState(false);
    const [addDialogOpen, setAddDialogOpen] = useState(false);
  
    const handleCloseInfo = () => {
        setInfoDialogOpen(false);
    };

    const handleCloseAdd = () => {
        setAddDialogOpen(false);
    };


    const reloadGrid = () => {
        setGridReload((prev) => !prev);
    };

    const renderDetailsButton = useCallback((params) => {
        const id = params.row.id;

        return (
            <div className='buttonsWrapper'>
                {
                    params.row.isActive?<Button
                    color="primary"
                    size="small"
                    onClick={() => {
                        navigator(`/company-details/${id}`)
                    }}
                >
                    <InfoIcon />
                </Button> : <></>  
                }
                <Button
                    color="error"
                    size="small"
                    onClick={(e) => {
                        e.stopPropagation();
                        setActiveState({id: id});
                        setPopupMessage(`Do you want to delete this package?`);
                        setDeletePopupOpened(true);
                    }}
                >
                    <DeleteIcon />
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
            field: 'maxUsersCount',
            headerName: 'Maximum Users Count',
            editable: false,
            flex: 1
        },
        {
            field: 'maxRecordersCount',
            headerName: 'Maximum Recorders Count',
            editable: false,
            flex: 1
        },
        {
            field: 'price',
            headerName: 'Price',
            editable: false,
            flex: 1
        },
        {
            field: 'currency',
            headerName: 'Currency',
            editable: false
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
    
    const handleRowClick = (params) => {
        console.log('Row clicked:', params.row);
        setSelectedRow(params.row);
        setInfoDialogOpen(true);
        // Handle the click event here, e.g., open a dialog, navigate to a new page, etc.
    };

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/packages`)).data;
            setRows(data);

            setLoading(false);
        }
        setRows([]);
        setLoading(true);
        getData()
    }, [gridReload])

    return <>
        <Box sx={{ 
            height: 780, 
            width: '100%',
        }} className="billing-wrapper">
        <Button sx={{marginBottom: 2}} onClick={() => setAddDialogOpen(true)} variant="outlined" color="success">Add new package</Button>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        rowHeight={70}
        hideFooterPagination
        hideFooterSelectedRowCount
        disableSelectionOnClick
        experimentalFeatures={{ newEditingApi: true }}
        onRowClick={handleRowClick}
      />
    </Box>
    <BillingPackageInfo open={infoDialogOpen} handleClose={handleCloseInfo} reloadGrid={reloadGrid} row={selectedRow}/>
    <BillingPackageAdd open={addDialogOpen} handleClose={handleCloseAdd} reloadGrid={reloadGrid}/>
    <ConfirmationPopup opened={deletePopupOpened} handleOk={async () => {
                    try {
                        await destroy(`${recorderApiUrl}/api/packages/${activeState.id}`);
                        setDeletePopupOpened(false);
                        setGridReload(!gridReload);
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message={popupMessage}/>
    </>
}

export default memo(BillingPage);