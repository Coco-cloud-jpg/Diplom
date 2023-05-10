import React, { memo, useEffect, useState } from 'react';
import './home-entrance.css';
import { CardContent, CircularProgress, FormControl, InputLabel, MenuItem, Select } from '@mui/material';

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';
import { get } from '../../helpers/axiosHelper';
import { recorderApiUrl } from '../../constants';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  indexAxis: 'y',
  responsive: true,
  scale: {
    ticks: {
      precision: 0
    }
  }
};

const HomeEntrance = () => {
    const [timeRange, setTimeRange] = useState(1);
    
    const [loading, setLoading] = useState(true);

    const [chartConfigs, setChartConfigs] = useState({});

    useEffect(() => {
      async function getData() {
          let data = (await get(`${recorderApiUrl}/api/entrance/chart/${timeRange}`)).data;
          console.log(data);
          setChartConfigs(
              {
                labels: data.map(item => item.holderName ), 
                datasets: [
                  {
                    label: 'Entrances',
                    data: data.map(item => item.entries),
                    borderWidth: 1,
                    backgroundColor: "#1976D2",
                    barThickness: 8
                  },
                  {
                    label: 'Warnings',
                    data: data.map(item => item.warnings),
                    borderWidth: 1,
                    backgroundColor: "rgba(240, 219, 108, 0.6)",
                    barThickness: 8
                  },
                  {
                    label: 'Violations',
                    data: data.map(item => item.violations),
                    borderWidth: 1,
                    backgroundColor: "rgba(211, 47, 47, 0.2)",
                    barThickness: 8
                  }
                ]
              })
              
          setLoading(false);
      }
      
      setLoading(true);
      getData();
    }, [timeRange])

    return (
        <>
            <div>
                <span>Entrances</span>
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
            <CardContent sx={{display: "flex", justifyContent: "center", alignItems: "center", flexDirection: "column", height: "100%"}}>
                <div className='home-entrance-wrapper'>
                    {loading? <CircularProgress />: <Bar options={options} data={chartConfigs} />}
                </div>
            </CardContent>
        </>
    );
  }
  
  export default memo(HomeEntrance);
