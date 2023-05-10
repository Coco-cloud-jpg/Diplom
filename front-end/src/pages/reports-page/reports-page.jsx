import { memo, useCallback, useEffect, useState } from "react";
import {
    TextField,
    Checkbox,
    FormControlLabel,
    Button,
    Select,
    MenuItem,
    Grid,
    Paper,
    FormControl,
    InputLabel,
    Card,
    CircularProgress,
  } from "@mui/material";
import './reports-page.css';
import { downloadFile, get, loadFile } from "../../helpers/axiosHelper";
import { recorderApiUrl } from "../../constants";
import { MobileDatePicker } from "@mui/x-date-pickers";
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { Document, Page, pdfjs } from 'react-pdf';
import pdfjsWorker from 'pdfjs-dist/build/pdf.worker.entry';

const ReportsPage = () => {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const [pdfData, setPdfData] = useState(null);

    const [inputRecorder, setInputRecorder] = useState("");
    const [recordersList, setRecordersList] = useState([]);
    const [reportLoading, setReportLoading] = useState(false);
    const [pdfUrl, setPdfUrl] = useState("");
    
    const loadReport = useCallback(async (e) => {
        e.preventDefault();
        
        if (inputRecorder == null || endDate == null || startDate == null)
            return;

        setReportLoading(true);
        let file = await loadFile(`${recorderApiUrl}/api/reports/report`, {
            startDate: startDate,
            endDate: endDate,
            recorderId: inputRecorder,
            includeViolatedScreenshots: includeViolatedScreenshots
        });
        
        setPdfUrl(URL.createObjectURL(file));
        setReportLoading(false);
    });

    useEffect(() => {
        async function getRecorders() {
            if (recordersList[0])
                return;

            try {
                let list = (await get(`${recorderApiUrl}/api/alerts/recorders`)).data;
                setRecordersList(list);   
            } catch (error) {
                console.log(error);
            }
        }
        
        getRecorders();
    }, []);

    const handleChange = (event, callback) => {
        callback(event.target.value);
    };
    
    const [includeViolatedScreenshots, setIncludeViolatedScreenshots] =
        useState(false);

    const handleIncludeViolatedScreenshotsChange = (event) => {
        setIncludeViolatedScreenshots(event.target.checked);
    };

    return <div className="reports-page-wrapper">
        <Card className="configuration">
            <h3>Configuration</h3>
            <div>
                <FormControl className="form-item" sx={{width: "100%", '&.Mui-focused': {color: "#0F2E2F"} }} size="small">
                    <InputLabel id="demo-simple-select-label">Recorders</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        id="demo-simple-select"
                        label="Recorders"
                        value={inputRecorder}
                        onChange={(e) =>  handleChange(e, setInputRecorder)}
                        >
                        <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                            key={null} value={null}>-
                        </MenuItem>
                        {recordersList.map(item => 
                        <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                        key={item.id} value={item.id}>{item.name}
                        </MenuItem>)
                        }
                    </Select>
                </FormControl>
                <div className="form-item">
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DemoContainer components={['DatePicker']} label="Static variant">
                            <MobileDatePicker label="From date:"
                                        views={["year", "month", "day"]}
                                        format="DD-MM-YYYY"
                                        className="date-picker"
                                        value={startDate}
                                        onChange={(newValue) => setStartDate(newValue)}/>
                        </DemoContainer>
                    </LocalizationProvider>
                </div>
                <div className="form-item">
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DemoContainer components={['DatePicker']}>
                            <MobileDatePicker label="To date:"
                                        className="date-picker"
                                        views={["year", "month", "day"]}
                                        format="DD-MM-YYYY"
                                        value={endDate}
                                        onChange={(newValue) => setEndDate(newValue)} />
                        </DemoContainer>
                    </LocalizationProvider>
                </div>  
                <FormControlLabel
                    className="form-item"
                    control={
                        <Checkbox
                        checked={includeViolatedScreenshots}
                        onChange={handleIncludeViolatedScreenshotsChange}
                        />
                    }
                    label="Include Violated Screenshots"
                />
            </div>
            
            <Button variant="contained" color="primary" onClick={loadReport}>Search</Button>
        </Card>
        <Card className="pdf">
            {reportLoading ? 
            <div>
                <CircularProgress /> 
            </div>:
            <div>
                <iframe src={pdfUrl}></iframe>
            </div>}
        </Card>
    </div>;
}

export default memo(ReportsPage);