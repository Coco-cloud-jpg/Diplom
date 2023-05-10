import React, { memo, useCallback, useEffect, useRef, useState } from 'react';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import { Pie } from 'react-chartjs-2';
import './home-apps-chart.css';
import { CardContent, CircularProgress, FormControl, InputLabel, List, ListItem, ListItemText, MenuItem, Select } from '@mui/material';
import { get } from '../../helpers/axiosHelper';
import { secondsToTime } from '../../helpers/dateTimeHelper';
import { recorderApiUrl } from '../../constants';

const AccessRequestTile = () => {
    useEffect(() => {
      async function getData() {
          /*const data = (await get(`${recorderApiUrl}/api/apps/chart/${timeRange}`)).data;

          if (data.length === 0)
            data.push({name: "No data", seconds: 0})*/
      }
      getData();
    }, [])

    return <div></div>;
}
  
export default memo(AccessRequestTile);
