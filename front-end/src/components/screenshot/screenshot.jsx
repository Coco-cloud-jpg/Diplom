import {memo, useCallback, useEffect, useState} from 'react';
import { Button, CircularProgress, List, ListItem, Modal } from '@mui/material';
import { Box } from '@mui/system';
import { get, patch, post } from '../../helpers/axiosHelper';
import { recorderApiUrl } from '../../constants';
import './screenshot.css';
import { useGridState } from '@mui/x-data-grid';

const style = {
    position: 'absolute',
    top: '50%',
    width: '70%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    textAlign: "center",
    boxShadow: 24,
    outline: 'none'
  };

const Screenshot = ({url, id, recorderId, updateGrid, mark, page}) => {
    const [clicked, setClicked] = useState(false);
    const [comment, setComment] = useState('');
    const [reloadComments, setReloadComments] = useState(false);
    const [commentArray, setCommentArray] = useState([]);

    const reportViolation = useCallback(async () => {
        try {
            let newMark = mark == 2 ? 0 : 2;
            await patch(`${recorderApiUrl}/api/screenshots/${recorderId}/${id}/${newMark}`);   
        } catch (error) {
            console.log(error);        
        }

        setClicked(false);
        updateGrid({page: page, pageSize: 3});
    });

    const handleSubmit = useCallback(async () => {
        await post(`${recorderApiUrl}/api/comments/${id}`, {Text: comment});
        setComment('');
        setReloadComments(!reloadComments);
      })

    useEffect(() => {
        async function fetch() {
            const data = (await get(`${recorderApiUrl}/api/comments/${id}`)).data;
            setCommentArray(data);
            console.log("ASDQWDQWDQW", data);
        }
        fetch();
    }, [reloadComments]);


    return <>
        <img
            src={url}
            alt="Screenshot"
            onClick={() => {setClicked(true)}} style={{cursor: "pointer"}}/>
        <Modal open={clicked} onClose={() => {setClicked(false)}} sx={{width: "100%"}}>
            <Box sx={style}>
                <div className='screenshot-wrapper'>
                    <div className="pop-up-image-wrapper">
                        <img src={url} alt="Screenshot" style={{ maxWidth: "100%", maxHeight: "calc(100vh - 64px)" }}/>
                    </div>
                    <div className='warnings-block'>
                        <h3>Comment</h3>
                        <div className='text-area-block'>
                            <textarea  placeholder="Input text here"  rows="5" cols="30" value={comment} onChange={(e) => setComment(e.target.value)} className="screenshot-textarea"></textarea>
                        </div>
                        <div className="submit-wrapper">
                            <Button variant="contained" onClick={handleSubmit}>Submit</Button>
                        </div>
                        <div className='comments'>
                            {commentArray.length > 0 ?
                            <ul>
                            {commentArray.map((item, i) => 
                                    <li key={i} className="comment-item">
                                        <h5>{item.posterName}</h5>
                                        <h6 className='date'>{item.datePosted}</h6>
                                        <p className='text'>
                                            {item.text}
                                        </p>
                                    </li>)
                            }
                            </ul>
                            :<div className='empty-comment-section'><p>No comments are present for this screenshot</p></div>
                            }
                        </div>
                    </div>
                </div>
            <Button color={mark == 2? "success" : "error"} variant="contained" onClick={reportViolation}>{mark == 2? "Restore mark" : "Report vioalation"}</Button>
            </Box>
        </Modal>
    </>
}

export default memo(Screenshot);