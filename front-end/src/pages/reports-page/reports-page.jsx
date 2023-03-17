import { memo, useEffect, useState } from "react";
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
  } from "@mui/material";
import DatePicker from "react-datepicker";
import DateTime from 'react-datetime';
import './reports-page.css';
import { get } from "../../helpers/axiosHelper";
import { recorderApiUrl } from "../../constants";

const ReportsPage = () => {
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());

    const [inputRecorder, setInputRecorder] = useState("");
    const [recordersList, setRecordersList] = useState([]);

    useEffect(() => {
        async function getRecorders() {
            if (recordersList[0])
                return;

            try {
                var list = (await get(`${recorderApiUrl}/api/alerts/recorders`)).data;
                setRecordersList(list);   
            } catch (error) {
                console.log(error);
            }
        }
        
        getRecorders();
    }, [recordersList]);

    const handleChange = (event, callback) => {
        callback(event.target.value);
    };
    
    const [includeViolatedScreenshots, setIncludeViolatedScreenshots] =
        useState(false);

    const [selectedDate, setSelectedDate] = useState(null);

    function handleDateChange(date) {
      setSelectedDate(date);
    }

    const handleIncludeViolatedScreenshotsChange = (event) => {
        setIncludeViolatedScreenshots(event.target.checked);
    };

    return <div className="reports-page-wrapper">
        <Card className="configuration">
            <FormControl sx={{ marginTop: 1, minWidth: 315, '&.Mui-focused': {color: "#0F2E2F"} }} size="small">
                <InputLabel id="demo-simple-select-label">Recorders</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    label="Recorders"
                    value={inputRecorder}
                    onChange={(e) =>  handleChange(e, setInputRecorder)}
                    >
                    <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                        key={null} value={null}>All
                    </MenuItem>
                    {recordersList.map(item => 
                    <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                    key={item.id} value={item.id}>{item.name}
                    </MenuItem>)
                    }
                </Select>
            </FormControl>
            <div>
                <label for="from">From date:</label>
                <DateTime
                    id="from"
                    onChange={handleDateChange}
                    value={selectedDate}
                    inputProps={{ placeholder: 'Select start date and time' }}
                />
            </div>
            <div>
                <label for="to">To date:</label>
                <DatePicker selected={startDate} onChange={(date) => setStartDate(date)} />
            </div>  
                <FormControlLabel
                  control={
                    <Checkbox
                    checked={includeViolatedScreenshots}
                    onChange={handleIncludeViolatedScreenshotsChange}
                    />
                  }
                  label="Include Violated Screenshots"
                />
                <Button variant="contained" color="primary">Search</Button>
        </Card>
        <Card className="pdf">
            la
        </Card>
    </div>;
}

export default memo(ReportsPage);