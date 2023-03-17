import {memo, useCallback, useEffect, useState} from 'react';
import { Button, CircularProgress, FormControl, InputLabel, LinearProgress, MenuItem, Modal, Select } from '@mui/material';
import { Box } from '@mui/system';
import { CloseFullscreen } from '@mui/icons-material';
import axios from 'axios';
import "./add-rule-popup.css";
import { get, patch, post, put } from '../../helpers/axiosHelper';
import { recorderApiUrl } from '../../constants';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    textAlign: "center",
    background: "#fff",
    boxShadow: 24,
  };

const AddRulePopup = ({opened, close, type, data, title}) => {
    const [loading, setLoading] = useState(false);
    const [inputWords, setInputWords] = useState("");
    const [inputEmail, setInputEmail] = useState("");
    const [inputRecorder, setInputRecorder] = useState("");
    const [recordersList, setRecordersList] = useState([]);

    useEffect(() => {
        async function getCountries() {
            if (recordersList[0])
                return;

            try {
                var list = (await get(`${recorderApiUrl}/api/alerts/recorders`)).data;
                setRecordersList(list);   
                var recorder = list.filter(item => item.name === data.toRecorder)[0];
                console.log(recorder);
                if (recorder != null)
                    setInputRecorder(recorder.id);
            } catch (error) {
                console.log(error);
            }
        }
        
        if (type === 2) {
            console.log(data);
            setInputEmail(data.sendToEmail);
            setInputWords(JSON.parse(data.serializedWords).join(", "));
        }

        getCountries();
    }, [recordersList]);

    const handleChange = (event, callback) => {
        callback(event.target.value);
    };

    const submit = async (e) => {
        e.preventDefault();

        if (inputEmail == "" || inputWords == "")
            return;

        setLoading(true);

        if (type === 1) {
            await post (`${recorderApiUrl}/api/alerts`, 
            {
                recorderId: inputRecorder || null, 
                sendToEmail: inputEmail,
                serializedWords: inputWords
            })
        }
        else if (type === 2) {
            await put(`${recorderApiUrl}/api/alerts/${data.id}`, 
            {
                recorderId: inputRecorder || null, 
                sendToEmail: inputEmail,
                serializedWords: inputWords
            })
        }
        else 
            return;
        
        closeAll();
    }

    const closeAll = () => {
        close();
        setLoading(false);
        setInputWords("");
        setInputEmail("");
        setInputRecorder("");
    }

    return <>
        <Modal open={opened} onClose={() => closeAll()} sx={{width: "100%"}} className="add-user">
            <Box sx={style}>
            <form>
                    <h2>{title}</h2>
                    <fieldset>
                        <ul>
                            <li className='words'>
                                <label htmlFor="words">Words *</label>
                                <p>provide words to search for below using coma as seperator</p>
                                <input type="text" id="words" value={inputWords} onChange={(e) => handleChange(e, setInputWords)} required/>
                            </li>
                            <li>
                                <label htmlFor="email">Email to send *</label>
                                <input type="email" id="email" value={inputEmail} onChange={(e) => handleChange(e, setInputEmail)} required/>
                            </li>
                            <li>
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
                                        key={null} value={null}>-</MenuItem>
                                        {recordersList.map(item => <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                                        key={item.id} value={item.id}>{item.name}</MenuItem>)}
                                    </Select>
                                </FormControl>
                            </li>
                        </ul>
                    </fieldset>
                    <button type='submit' className="primaryButton" onClick={submit}>Create</button>
                </form>
            <div className='progress'>
             {loading && <LinearProgress />}
            </div>
            </Box>
        </Modal>
    </>
}

export default memo(AddRulePopup);