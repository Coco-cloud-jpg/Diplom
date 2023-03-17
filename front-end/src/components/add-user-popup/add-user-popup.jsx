import {memo, useCallback, useEffect, useState} from 'react';
import { Button, CircularProgress, LinearProgress, Modal } from '@mui/material';
import { Box } from '@mui/system';
import { CloseFullscreen } from '@mui/icons-material';
import "./add-user-popup.css";
import axios from 'axios';
import { post } from '../../helpers/axiosHelper';
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

const AddUserPopup = ({opened, close, recorderId}) => {
    const [isSuccessRegistration, setIsSuccessRegistration] = useState(false);
    const [loading, setLoading] = useState(false);
    const [inputHolderName, setInputHolderName] = useState("");
    const [inputHolderSurname, setInputHolderSurname] = useState("");
    const handleChange = (event, callback) => {
        callback(event.target.value);
    };

    const submit = async (e) => {
        e.preventDefault();
        setLoading(true);

        var data = await post (`${recorderApiUrl}/api/recordings`, 
            {
                holderName: inputHolderName, 
                holderSurname: inputHolderSurname
            })
        console.log(data.status);
        if (data.status) {
            setIsSuccessRegistration(true);
        }
        
        setLoading(false);
    }

    const closeAll = () => {
        close();
        setIsSuccessRegistration(false);
        setInputHolderName("");
        setInputHolderSurname("");
    }

    return <>
        <Modal open={opened} onClose={() => closeAll()} sx={{width: "100%"}} className="add-user">
            <Box sx={style}>
            {isSuccessRegistration ?
                <div>
                    Here you can download recorder.
                    <Button>Download</Button>
                </div>
            :
                <form>
                    <h2>Recorder Registration</h2>
                    <fieldset>
                        <ul>
                            <li>
                                <label htmlFor="name">Holder Name</label>
                                <input type="text" id="name" value={inputHolderName} onChange={(e) => handleChange(e, setInputHolderName)} required minLength="3"/>
                            </li>
                            <li>
                                <label htmlFor="surname">Holder Surname</label>
                                <input type="text" id="surname" value={inputHolderSurname} onChange={(e) => handleChange(e, setInputHolderSurname)} required minLength="3"/>
                            </li>
                        </ul>
                    </fieldset>
                    <button className="primaryButton" onClick={submit}>Create</button>
                </form>
            }
            <div className='progress'>
             {loading && <LinearProgress />}
            </div>
            </Box>
        </Modal>
    </>
}

export default memo(AddUserPopup);