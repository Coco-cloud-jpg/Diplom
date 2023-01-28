import {useState} from 'react';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';

const ConfirmationPopup = ({opened, handleOk, handleClose, message}) => {
  return (
    <Dialog open={opened} onClose={handleClose}>
        <DialogTitle>Alert</DialogTitle>
        <DialogContent>
        <DialogContentText>
            {message}
        </DialogContentText>
        </DialogContent>
        <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button onClick={handleOk}>Ok</Button>
        </DialogActions>
    </Dialog>
  );
}

export default ConfirmationPopup;