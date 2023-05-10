import { Card, CardContent, CardHeader, FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import HomeAppsChart from '../../components/home-apps-chart/home-apps-chart';
import HomeEntrance from '../../components/home-entrance/home-entrance';
import ChartComponent from '../../components/screenshots-chart/screenshots-chart';
import './home-page.css';

const HomePage = () => {
    return <div className='home-wrapper'>
            <div>
                <Card className='card'>
                    <ChartComponent />
                </Card>
                <Card className='card'>
                    <HomeEntrance />
                </Card>
            </div>
            <div>
                <Card className='card'>
                    <HomeAppsChart />
                </Card>
                <Card className='card'>
                    <div>asd</div>
                </Card>
            </div>
    </div>
}

export default HomePage;