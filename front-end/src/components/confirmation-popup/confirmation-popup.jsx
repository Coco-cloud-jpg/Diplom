import {useState} from 'react';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { Checkbox, FormControlLabel } from '@mui/material';

const ConfirmationPopup = ({opened, handleOk, handleClose, message, handleDelete}) => {
  const [destroy, setDestroy] = useState(false);

  return (
    <Dialog open={opened} onClose={handleClose}>
        <DialogTitle>Alert</DialogTitle>
        <DialogContent>
        <DialogContentText>
            {message}
        </DialogContentText>
        {handleDelete ? <FormControlLabel
                    className="form-item"
                    control={
                        <Checkbox
                        checked={destroy} onChange={(e) => setDestroy(e.target.checked)}
                        />
                    }
                    label="Totally Delete"
                />: <></>}
        </DialogContent>
        <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button onClick={destroy ? handleDelete:handleOk}>Ok</Button>
        </DialogActions>
    </Dialog>
  );
}

export default ConfirmationPopup;