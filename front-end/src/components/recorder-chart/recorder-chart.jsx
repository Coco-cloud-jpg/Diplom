import { CircularProgress } from "@mui/material";
import { memo, useEffect, useState } from "react"
import { recorderApiUrl } from "../../constants";
import { get } from "../../helpers/axiosHelper";
import { secondsToTime } from "../../helpers/dateTimeHelper";
import "./recorder-chart.css";

const secondsInDay = 60*60*8;

const RecorderChart = ({recoredId}) => {
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState([]);

    useEffect(() => {
        async function getData() {
            let data = (await get(`${recorderApiUrl}/api/entrance/week/${recoredId}`)).data;
            //chartData.labels = Object.keys(data);
            setData(data);
            setLoading(false);
        }
        setLoading(true);
        getData();
    }, []);

    return <>
        {loading ? 
        <div className="wrapper"><CircularProgress /></div>:
        <div className="chart">{Object.keys(data).map(item => 
          <div key={item}>
            <div className="title">{item}</div>
            <div title={secondsToTime(data[item])} className="bar-wrapper">
              <div className="bar" style={{width: `${data[item]/secondsInDay * 100}%`}}></div>
            </div>
            <div className="plus">{data[item]/secondsInDay > 1 && "+"}</div>
          </div>
        )}</div>
        }
    </>
}

export default memo(RecorderChart);