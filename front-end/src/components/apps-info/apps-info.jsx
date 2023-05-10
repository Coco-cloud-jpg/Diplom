import { List, ListItem, ListItemText, Typography } from "@mui/material";
import { memo, useCallback, useEffect, useState } from "react"
import { recorderApiUrl } from "../../constants";
import { get } from "../../helpers/axiosHelper";
import './apps-info.css';

const red = "#ca2b1d";
const green = "#46992c";
const yellow = "#f4da55";

const AppsInfo = ({recoredId}) => {
    const [data, setData] = useState([]);
    const [max, setMax] = useState(0);

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/apps/${recoredId}`)).data;
            setData(data);
            setMax(data.reduce((accumulator, item) => accumulator + item.seconds, 0))
        }
        getData();
    }, []);

    const color = useCallback((seconds) => {
        const percent = seconds / max;
        
        return percent > 0.5 ? red: percent > 0.25 ?yellow: green;
    });

    return <>{data.length > 0 ?<List sx={{
        width: '100%',
        maxWidth: 360,
        bgcolor: 'background.paper',
        position: 'relative',
        overflow: 'auto',
        maxHeight: 180
      }}>
        {data.map((item, i) => 
            <ListItem key={i} title={`${(item.seconds / max * 100).toFixed(2)}% time spent`}>
                <img src={`data:image/png;base64,${item.iconBase64}`}/>
                <ListItemText className="list-item-text"
                secondary={
                    <>
                    <span className="title">{item.name}</span>
                    <span className="graph-wrapper">
                        <span className="graph" style={{width: `${item.seconds / max * 100}%`, background: `${color(item.seconds)}`}}></span>
                    </span>
                    </>
                }
                />
            </ListItem>)}
    </List>:<div className="app-data-empty">No App Data</div>}</>;
}

export default memo(AppsInfo);