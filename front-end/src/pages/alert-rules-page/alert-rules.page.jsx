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
import EditIcon from '@mui/icons-material/Edit';
import "./alert.rules-page.css";
import AddRulePopup from '../../components/add-rule-popup/add-rule-popup';
import { recorderApiUrl } from '../../constants';

const AlertRulesPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [deletePopupOpened, setDeletePopupOpened] = useState(false);
    const [popupMessage, setPopupMessage] = useState("Do you want to disable this recorder?");
    const [idToDelete, setIdToDelete] = useState("");
    const [includeDeleted, setIncludeDeleted] = useState(false);
    const [gridReload, setGridReload] = useState(false);
    const [addRecord, setAddRecord] = useState(false);
    const [editRecord, setEditRecord] = useState(false);
    const [selectedRow, setSelectedRow] = useState({});
    const [controller, setController] = useState({
        page: 0,
        pageSize: 5
      });

    console.log("render")
    const renderDetailsButton = useCallback((params) => {
        const id = params.row.id;
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
                    color="error"
                    size="small"
                    onClick={() => {
                        setPopupMessage(`Do you want to delete this rule?`);
                        setDeletePopupOpened(true);
                        setIdToDelete(id);
                    }}
                >
                    <DeleteIcon />
                </Button>
            </div>
        )
    }, []);

    const renderWordsSection = useCallback((params) => {
        const words = params.row.serializedWords;
        return (
            <div className='wordsWrapper'>
                {JSON.parse(words).map(item => <div>{item}</div>)}
            </div>
        )
    }, []);

    const columns = [
        { 
            field: 'id', 
            headerName: 'ID',
            flex: 2
        },
        {
          field: 'toRecorder',
          headerName: 'Assigned To Recorder',
          editable: false,
          flex: 1
        },
        {
            field: 'sendToEmail',
            headerName: 'Send To Email',
            editable: false,
            flex: 1
        },
        {
            field: 'serializedWords',
            headerName: 'Words',
            editable: false,
            flex: 1,
            renderCell: renderWordsSection
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

    const handlePageChange = (pagenumber) => {
        console.log(pagenumber);
        setController({
          ...controller,
          page: pagenumber
        });
    };

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/alerts?page=${controller.page}&pageSize=${controller.pageSize}`)).data;
            setRows(data);

            setLoading(false);
        }
        setRows([]);
        setLoading(true);
        getData()
    }, [controller, includeDeleted, setIncludeDeleted, gridReload])

    return <>
        <Box sx={{ 
            height: 600, 
            width: '100%'
        }}>
        <div className='recorders-page-panel'>
            <Button onClick={() => setAddRecord(true)} variant="outlined" color="success">Add new</Button>
            <AddRulePopup type={1} title="Add Rule" opened={addRecord} close={() => {
                setAddRecord(false);
                setGridReload(!gridReload);
            }
                }/>
        {editRecord && <AddRulePopup type={2} title="Edit Rule" data={selectedRow} opened={editRecord} close={() => {
                        setEditRecord(false);
                        setGridReload(!gridReload);
                    }
                    }/>}
        </div>
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        pageSize={5}
        rowHeight={75}
        onPageChange={handlePageChange}
        disableSelectionOnClick
        experimentalFeatures={{ newEditingApi: true }}
      />
    </Box>
    <ConfirmationPopup opened={deletePopupOpened} handleOk={async () => {
                    try {
                        await destroy(`${recorderApiUrl}/api/alerts/${idToDelete}`);
                        setDeletePopupOpened(false);
                        setGridReload(!gridReload);
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }} handleClose={() => {setDeletePopupOpened(false)}} message={popupMessage}/>
    </>
}

export default memo(AlertRulesPage);