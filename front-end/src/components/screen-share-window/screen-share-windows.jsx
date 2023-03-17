import {memo, useEffect, useState} from 'react';
import { Button, CircularProgress, Modal } from '@mui/material';
import { Box } from '@mui/system';
import * as signalR from "@microsoft/signalr";
import './screen-share-windows.css';
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

const ScreenShareWindow = ({opened, close, recorderId}) => {
    const [connection, setConnection] = useState(null);
    const [image, setImage] = useState(null);
    const [recorderAvailable, setRecorderAvailable] = useState(true);

    useEffect(() => {
        setTimeout(() => {
            if (image == null)
                setRecorderAvailable(false);
        }, 5000);

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${recorderApiUrl}/screenShare`)
            .build();
        setConnection(newConnection);

        return () => {
            if (newConnection) {
                newConnection.stop();
                setConnection(null);
            }
        };
    }, []);

    useEffect(() => {
        if (connection) {
            startConnection();
          connection.on("ReceiveMessage", message => {
            console.log("Received message:", message);
          });
          connection.on("ReceiveImage", message => {
                setImage(`data:image/jpeg;base64,${message}`)
          })
        }
      }, [connection]);

    const sendMessage = () => {
        connection.invoke("SendMessage", recorderId)
          .catch(err => console.error(err));
    }

    const startConnection = () => {
        connection.start()
        .then(() => sendMessage())
        .catch(err => console.error(err));
    }

    const end = () => {
        close();
    }

    return <>
        <Modal open={opened} onClose={() => end()} sx={{width: "100%"}}>
            <Box sx={style}>
                {
                    image != null ?
                    <div><img src={image} alt="Screenshot" style={{ maxWidth: "100%", maxHeight: "calc(100vh - 64px)" }}/></div>:
                    <div>
                        {
                        recorderAvailable ?
                        <div className='connecting'>
                            Connecting...
                            <div>
                                <CircularProgress />
                            </div>
                        </div>:
                        <div className='no-connection'>Recorder isn't available</div>
                        }
                    </div>
                }
            </Box>
        </Modal>
    </>
}

export default memo(ScreenShareWindow);