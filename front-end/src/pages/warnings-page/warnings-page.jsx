import { Box, Button, Card, CardHeader, Checkbox, FormControlLabel, Radio, RadioGroup, TextareaAutosize, TextField } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { memo, useCallback, useEffect, useState } from "react";
import { recorderApiUrl } from "../../constants";
import { get, post } from "../../helpers/axiosHelper";
import "./warnings-page.css";

const WarningsPage = () => {
    const [rows, setRows] = useState([]);
    const [loading, setLoading] = useState(true);
    const [gridReload, setGridReload] = useState(false);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(0);
    const [comment, setComment] = useState('');
    const [checked, setChecked] = useState(false);
    const [commentArray, setCommentArray] = useState([]);
    const [commentsReload, setReloadComments] = useState(false);

    useEffect(() => {
        if (!rows[0])
            return;
            
        async function fetch() {
            const data = (await get(`${recorderApiUrl}/api/comments/${rows[0]?.id}`)).data;
            setCommentArray(data);
        }
        fetch();
    }, [commentsReload]);

    const [type, setType] = useState("0");

    const handleSubmit = useCallback(async () => {
        await post(`${recorderApiUrl}/api/warnings/${rows[0]?.id}`, {Text: comment, Mark: type, PostComment: checked});
        setComment('');
        setType('0');
        setChecked(false);
        setGridReload(!gridReload);
        console.log('Comment submitted:', comment);
      })
    
      const handleCancel = () => {
      }
    
      const handleChange = (event) => {
        console.log("Asdasdasd");
        comment += "1"; 
        console.log(event.target.value);
        setComment(event.target.value);
      }

    const renderWordsSection = useCallback((params) => {
        const base64 = params.row.base64;
        return (
            <div className="image-wrapper">
                <img src={base64}/>
            </div>
        )
    }, []);

    const columns = [
        {
            field: 'base64',
            headerName: 'Warned screenshot',
            editable: false,
            flex: 1,
            sortable: false,
            filterable: false,
            renderCell: renderWordsSection
        }
      ];
    

    useEffect(() => {
        async function getData() {
            var response = (await get(`${recorderApiUrl}/api/warnings?page=${page}`)).data;
            setRows(response.data);
            setTotalCount(response.total);
            setReloadComments(!commentsReload);
            setLoading(false);
        }
        setLoading(true);
        getData()
    }, [gridReload, page])

    const handlePageChange = (pagenumber) => {
        setPage(pagenumber);
      };

    return <Box sx={{ 
        height: 800, 
        width: '100%',
        '.row-custom:hover': {
            bgcolor: "#fff !important",
        },
        '.row-custom:focus': {
            bgcolor: "#fff !important"
        },
        display: "flex"
    }}>
        <DataGrid
            rows={rows}
            columns={columns}
            loading={loading}
            pageSize={1}
            pagination
            rowHeight={650}
            rowCount={totalCount}
            paginationMode="server"
            onPageChange={handlePageChange}
            disableSelectionOnClick
            disableColumnMenu
            disableRowSelection
            experimentalFeatures={{ newEditingApi: true }}
            getRowClassName={(params) => `row-custom`}
            sx={{width: "70%"}}
        />
        <div className="menu">
            <Card className="warnings-card">
                <h3>Comment</h3>
                <textarea placeholder="Input text here" className="warnings-text" rows="10" cols="50" value={comment} onChange={(e) => setComment(e.target.value)}></textarea>
                <div className="buttons">
                    <div>
                        <RadioGroup aria-label="report-restore" name="report-restore" value={type} onChange={(e) => setType(e.target.value)}>
                            <FormControlLabel value="2" control={<Radio />} label="Report Violation" />
                            <FormControlLabel value="0" control={<Radio />} label="Restore Mark" />
                        </RadioGroup>
                    </div>
                    <FormControlLabel
                            control={<Checkbox checked={checked} onChange={(e) => setChecked(e.target.checked)} />}
                            label="Post Comment"
                            disabled={false}
                        />
                    <div className="submit-wrapper">
                        <Button variant="contained" onClick={handleSubmit}>Submit</Button>
                    </div>
                </div>
            </Card>
            <Card sx={{width: '100%', marginTop: 2}}>
                <div className='warnings-comments'>
                        {commentArray.length > 0? 
                            <ul>
                                {commentArray.map((item, i) => 
                                <li key={i} className="comment-item">
                                    <h5>{item.posterName}</h5>
                                    <h6 className='date'>{item.datePosted}</h6>
                                    <p className='text'>
                                        {item.text}
                                    </p>
                                </li>
                                )}
                            </ul>:
                            <div className='empty-comment-section'><p>No comments are present for this screenshot</p></div>}
                </div>
            </Card>
        </div>
    </Box>
}

export default memo(WarningsPage);