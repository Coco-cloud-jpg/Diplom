import { Button, Switch, TextField } from '@mui/material';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { useState } from 'react';
import { memo } from 'react';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { destroy, get, patch } from '../../helpers/axiosHelper';
import DeleteIcon from '@mui/icons-material/Delete';
import AutorenewIcon from '@mui/icons-material/Autorenew';
import ConfirmationPopup from '../../components/confirmation-popup/confirmation-popup';
import { useCallback } from 'react';
import AddUserPopup from '../../components/add-user-popup/add-user-popup';
import { identityApiUrl } from '../../constants';
import { useLocalStorage } from "../../hooks/useLocalStorage";
import AddSiteUser from '../../components/add-site-user/add-site-user';
import EditIcon from '@mui/icons-material/Edit';

const UsersPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [popupMessage, setPopupMessage] = useState("Do you want to disable this user?");
    const [activeState, setActiveState] = useState({id: "", isActive: false});
    const [includeDeleted, setIncludeDeleted] = useState(false);
    const [gridReload, setGridReload] = useState(false);
    const [addUserPopup, setAddUserPopup] = useState(false);
    const navigate = useNavigate();
    const [tokens, setTokens] = useLocalStorage("tokens", null);
    const [editRecord, setEditRecord] = useState(false);
    const [selectedRow, setSelectedRow] = useState({});

    console.log("render")
    const renderDetailsButton = useCallback((params) => {
        const id = params.row.id;
        const isActive = params.row.isActive;

        return (
            <div className='buttonsWrapper'>
                <Button
                    color="primary"
                    size="small"
                    className='button-update'
                    onClick={() => {
                        setEditRecord(true);
                        setSelectedRow(params.row);
                    }}
                >
                    <EditIcon />
                </Button>
                <Button
                    color={isActive? "error": "success"}
                    size="small"
                    onClick={() => {
                        setActiveState({id: id, isActive: isActive});
                        setPopupMessage(`Do you want to ${isActive?"disable": "activate"} this user?`);
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
          field: 'firstName',
          headerName: 'First name',
          editable: false,
          flex: 1
        },
        {
          field: 'lastName',
          headerName: 'Last name',
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
            field: 'role',
            headerName: 'Role',
            editable: false,
            flex: 1
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
    
    const handleDisable = async () => {
        try {
            const response = (await patch(`${identityApiUrl}/api/users/status-toggle/${activeState.id}`)).data;

            if (response.toLogout) {
                setTokens(null); 
                navigate("/login")
            }
            else {
                setDeletePopupOpened(false);
                setGridReload(!gridReload);
            }
        }
        catch (ex) {
            console.log(ex);
        }
    };

    const handleDelete = async () => {
        try {
            const response = (await destroy(`${identityApiUrl}/api/users/${activeState.id}`)).data;

            if (response.toLogout) {
                setTokens(null); 
                navigate("/login")
            }
            else {
                setDeletePopupOpened(false);
                setGridReload(!gridReload);
            }
        }
        catch (ex) {
            console.log(ex);
        }
    };
    
    useEffect(() => {
        async function getData() {
            let data = (await get(`${identityApiUrl}/api/users?includeDeleted=${includeDeleted}`)).data;
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
        <div style={{display: 'flex', justifyContent: 'space-between', marginBottom: '15px'}}>
            <div><Switch onClick={(e) => {setIncludeDeleted(!includeDeleted)}}/>Include disabled</div>
            <Button onClick={() => setAddUserPopup(true)} variant="outlined" color="success">Add new user</Button>
            <AddSiteUser opened={addUserPopup} close={() => {
                setAddUserPopup(false);
            }}/>
            <AddSiteUser type={2} title="Edit Rule" data={{...selectedRow}} opened={editRecord} close={() => {
                        setEditRecord(false);
                    }
                    }/>
        </div>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        autoPageSize
        rowHeight={73}
        disableSelectionOnClick
        experimentalFeatures={{ newEditingApi: true }}
        getRowClassName={(params) => `isActive--${params.row.isActive}`}
      />
    </Box>
    <ConfirmationPopup opened={deletePopupOpened} handleOk={handleDisable} handleDelete={activeState.isActive ? handleDelete: null} handleClose={() => {setDeletePopupOpened(false)}} message={popupMessage}/>
    </>
}

export default memo(UsersPage);