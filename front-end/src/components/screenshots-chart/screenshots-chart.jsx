import React, { memo, useEffect, useState } from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import './screenshots-chart.css'
import { CardContent, CircularProgress, FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import {get} from '../../helpers/axiosHelper';
import { recorderApiUrl } from '../../constants';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  responsive: true,
  plugins: {
    legend: {
      position: 'top',
    }
  },
  scale: {
    ticks: {
      precision: 0
    }
  }
};

const ScreenshotChart = () => {
    const [timeRange, setTimeRange] = useState(1);
    const [loading, setLoading] = useState(true);

    const [chartConfigs, setChartConfigs] = useState({});

    useEffect(() => {
      async function getData() {
          var data = (await get(`${recorderApiUrl}/api/screenshots/chart/${timeRange}`)).data;

          setChartConfigs(
              {
                labels: data.map(item => item.datePart), 
                datasets: [
                  {
                    label: 'Screenshots',
                    data: data.map(item => item.data),
                    borderColor: '#1976D2',
                    backgroundColor: '#1976D2',
                  },
                ]
              })
              
          setLoading(false);
      }
      
      setLoading(true);
      getData();
    }, [timeRange])

  return (<>
            <div>
                <span>Screenshots count</span>
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
                <div className='screenshots-chart-wrapper'>
                    {
                      loading ? <CircularProgress />
                      :<Line data={chartConfigs} options={options} />
                    }
                </div>
            </CardContent>
        </>
  );
}

export default memo(ScreenshotChart);