import {memo, useCallback, useState} from 'react';
import { Button, Modal } from '@mui/material';
import { Box } from '@mui/system';
import { patch } from '../../helpers/axiosHelper';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    textAlign: "center",
    boxShadow: 24,
  };

const Screenshot = ({url, id, recorderId, updateGrid, mark, page}) => {
    const [clicked, setClicked] = useState(false);
    const reportViolation = useCallback(async () => {
        try {
            let newMark = mark == 2 ? 0 : 2;
            await patch(`https://localhost:44375/api/screenshots/${recorderId}/${id}/${newMark}`);   
        } catch (error) {
            console.log(error);        
        }

        setClicked(false);
        updateGrid({page: page, pageSize: 3});
    });

    return <>
        <img
            src={url}
            alt="Screenshot"
            onClick={() => {setClicked(true)}} style={{cursor: "pointer"}}/>
        <Modal open={clicked} onClose={() => {setClicked(false)}} sx={{width: "100%"}}>
            <Box sx={style}>
            <div className="pop-up-image-wrapper"><img src={url} alt="Screenshot" style={{ maxWidth: "100%", maxHeight: "calc(100vh - 64px)" }}/></div>
            <Button color={mark == 2? "success" : "error"} variant="contained" onClick={reportViolation}>{mark == 2? "Restore mark" : "Report vioalation"}</Button>
            </Box>
        </Modal>
    </>
}

export default memo(Screenshot);