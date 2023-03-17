import { ScreenshotMonitor } from "@mui/icons-material";
import { CircularProgress } from "@mui/material";
import { memo, useEffect, useState } from "react"
import { get } from "../../helpers/axiosHelper";
import { secondsToTime } from "../../helpers/dateTimeHelper";
import MouseIcon from '@mui/icons-material/Mouse';
import KeyboardIcon from '@mui/icons-material/Keyboard';
import "./recorder-details-tile.css";
import AppsInfo from "../apps-info/apps-info";
import { recorderApiUrl } from "../../constants";

const RecorderDetailsTile = ({recoredId}) => {
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState({});

    useEffect(() => {
        async function getData() {
            var data = (await get(`${recorderApiUrl}/api/recordings/${recoredId}`)).data;
           
            console.log(data);

            setData(data);
            setLoading(false);
        }
        setLoading(true);
        getData();
    }, []);

    return <>
        {loading ? 
            <div className="wrapper"><CircularProgress /></div>:
            <div className="info">
                <div className="pheripheral-wrapper">
                    <div className="screenshotsCount" title="Screenshots today">
                        <ScreenshotMonitor  style={{color: "#1976D2", fontSize: 50, margin: 10}}/>
                        {data.screenshots}
                    </div>
                    <div className="pheripherals">
                        <div className="mouse-info" title={`Mouse activity - ${(data.mouseActivity*100).toFixed(2)}%`}>
                            <MouseIcon sx={{color: "#1976D2", fontSize: 30}}/>
                            <div className="pheripheralWrapper">
                                <div className="pheripheralInner" style={{width: `${data.mouseActivity*100}%`}}>
                                </div>
                            </div>
                        </div>
                        <div className="keyboard-info" title={`Keyboard activity - ${(data.keyboardActivity*100).toFixed(2)}%`}>
                            <KeyboardIcon sx={{color: "#1976D2", fontSize: 30}}/>
                            <div className="pheripheralWrapper">
                                <div className="pheripheralInner" style={{width: `${data.keyboardActivity*100}%`}}></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="apps-info">
                        <AppsInfo recoredId={recoredId}/>
                    </div>
            </div>
        }
    </>;
}

export default memo(RecorderDetailsTile);