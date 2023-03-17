import React, { memo, useCallback, useEffect, useRef, useState } from 'react';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import { Pie } from 'react-chartjs-2';
import './home-apps-chart.css';
import { CardContent, CircularProgress, FormControl, InputLabel, List, ListItem, ListItemText, MenuItem, Select } from '@mui/material';
import { get } from '../../helpers/axiosHelper';
import { secondsToTime } from '../../helpers/dateTimeHelper';
import { recorderApiUrl } from '../../constants';
ChartJS.register(ArcElement, Tooltip, Legend);


const red = "#ca2b1d";
const green = "#46992c";
const yellow = "#f4da55";

const HomeAppsChart = () => {
    const [timeRange, setTimeRange] = useState(1);
    const [list, setList] = useState([]);
    const [max, setMax] = useState(0);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
      async function getData() {
          var data = (await get(`${recorderApiUrl}/api/apps/chart/${timeRange}`)).data;

          if (data.length === 0)
            data.push({name: "No data", seconds: 0})

          setList(data);

          setMax(data.reduce((accumulator, item) => accumulator + item.seconds, 0) || 1);

          setLoading(false);
      }
      
      setLoading(true);
      getData();
    }, [timeRange])

  const color = useCallback((seconds) => {
      var percent = seconds / max;
      
      return percent > 0.5 ? red: percent > 0.25 ?yellow: green;
  });

    return <>
            <div>
                <span>Apps usage among all</span>
                <FormControl>
                    <InputLabel id="demo-simple-select-label">Time range</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        id="demo-simple-select"
                        value={timeRange}
                        label="Time range"
                        onChange={(e) => setTimeRange(e.target.value)}
                    >
                        <MenuItem value={1}>Today</MenuItem>
                        <MenuItem value={2}>This week</MenuItem>
                        <MenuItem value={3}>This month</MenuItem>
                    </Select>
                </FormControl>
            </div>
              {loading ? <div className='home-apps-wrapper'><div><CircularProgress /></div></div>: 
              <List sx={{
                width: '100%',
                bgcolor: 'background.paper',
                position: 'relative',
                overflow: 'auto',
                maxHeight: 751
              }}>
                {list.map((item, i) => 
                    <ListItem key={i} title={`${secondsToTime(item.seconds)} (${(item.seconds / max * 100).toFixed(2)}% time spent)`}>
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
            </List>
            }
    </>;
}
  
 export default memo(HomeAppsChart);
