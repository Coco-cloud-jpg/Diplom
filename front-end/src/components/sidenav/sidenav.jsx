import {memo, useState, forwardRef} from 'react';
import PropTypes from 'prop-types';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import Divider from '@mui/material/Divider';
import Drawer from '@mui/material/Drawer';
import IconButton from '@mui/material/IconButton';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import MailIcon from '@mui/icons-material/Mail';
import MenuIcon from '@mui/icons-material/Menu';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { useLocation, useNavigate } from 'react-router-dom';
import LogoutIcon from '@mui/icons-material/Logout';
import HomeIcon from '@mui/icons-material/Home';
import ScreenshotMonitorIcon from '@mui/icons-material/ScreenshotMonitor';
import { useLocalStorage } from '../../hooks/useLocalStorage';
import "./sidenav.css";
import PersonIcon from '@mui/icons-material/Person';
import CircleNotificationsIcon from '@mui/icons-material/CircleNotifications';
import WarningIcon from '@mui/icons-material/Warning';
import SummarizeIcon from '@mui/icons-material/Summarize';
import { get } from '../../helpers/axiosHelper';
import { useEffect } from 'react';
import ApartmentIcon from '@mui/icons-material/Apartment';
import CreditCardIcon from '@mui/icons-material/CreditCard';
import GroupIcon from '@mui/icons-material/Group';
import { identityApiUrl } from '../../constants';
import { Dialog, DialogContent, DialogTitle, Paper, Popover, Slide } from '@mui/material';
import AccountInfo from '../account-info/account-info';

const navData = [
        {
            id: 0,
            icon: <HomeIcon style={{color: "#fff"}}/>,
            text: "Home",
            link: "/home"
        },
        {
            id: 1,
            icon: <ScreenshotMonitorIcon  style={{color: "#fff"}}/>,
            text: "Recorders",
            link: "/recorders"
        },
        {
              id: 2,
              icon: <CircleNotificationsIcon  style={{color: "#fff"}}/>,
              text: "Alert Rules",
              link: "/alerts"
        },
        {
              id: 3,
              icon: <WarningIcon  style={{color: "#fff"}}/>,
              text: "Warnings",
              link: "/warnings"
        },
        {
              id: 4,
              icon: <SummarizeIcon  style={{color: "#fff"}}/>,
              text: "Reports",
              link: "/reports"
        },
        {
              id: 5,
              icon: <HomeIcon style={{color: "#fff"}}/>,
              text: "Home",
              link: "/home-admin"
        },
        {
              id: 6,
              icon: <ApartmentIcon style={{color: "#fff"}}/>,
              text: "Companies",
              link: "/companies"
        },
        {
              id: 7,
              icon: <CreditCardIcon style={{color: "#fff"}}/>,
              text: "Billing",
              link: "/billing"
        },
        {
              id: 8,
              icon: <GroupIcon style={{color: "#fff"}}/>,
              text: "Users",
              link: "/users"
        },
    ]

const drawerWidth = 240;

const Transition = forwardRef(function Transition(props, ref) {
  return <Slide direction="up" ref={ref} {...props} />;
});

const SideNav = (props) => {
  const { window } = props;
  const [mobileOpen, setMobileOpen] = useState(false);
  const location = useLocation();
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const [currentUserRoutes, setCurrentUserRoutes] = useState([]);
  const navigate = useNavigate();
  const [userInfo, setUserInfo] = useState({});

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  useEffect(() => {
       async function getAccountInfo() {
           try {
               const data = (await get(`${identityApiUrl}/api/account`)).data;
               const routes = data.routes;
               setCurrentUserRoutes(navData.filter(item => routes.includes(item.link)));
               setUserInfo(data.userInfo);
           }
           catch (e){
               console.log("ehere");
               console.log(e);
           }
       }

       getAccountInfo();
  }, [])

  const drawer = (
    <div className="drawer">
      <div>
        <List>
          {currentUserRoutes.map((item) => (
            <ListItem key={item.text} disablePadding sx={{ marginTop: 1}} className={(location.pathname.startsWith(item.link)?"active":"")}>
              <ListItemButton onClick={() => {navigate(`${item.link.toLowerCase()}`)}} sx={{position: 'relative'}}>
                <ListItemIcon>
                  {item.icon}
                </ListItemIcon>
                <ListItemText primaryTypographyProps={{ style: {color: "#fff"}}} primary={item.text} />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
        <Divider />
        <List>
          <ListItem disablePadding>
              <ListItemButton onClick={() => {setTokens(null); navigate("/login")}}>
                <ListItemIcon >
                  <LogoutIcon style={{color: "#fff"}}/>
                </ListItemIcon>
                <ListItemText  primaryTypographyProps={{ style: {color: "#E6F4F1"} }} primary={"Log out"} />
              </ListItemButton>
            </ListItem>
        </List>
      </div>
      <div></div>
    </div>
  );

  const container = window !== undefined ? () => window().document.body : undefined;

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{
          width: { sm: `calc(100% - ${drawerWidth}px)` },
          ml: { sm: `${drawerWidth}px` },
          background: "#1976D2",
          boxShadow: "none",
          borderBottom: "1px solid rgba(0, 0, 0, 0.12)",
          borderBottomWidth: "thin",
        }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { sm: 'none' } }}
          >
            <MenuIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
        aria-label="mailbox folders"
      >
        <Drawer
          container={container}
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true,
          }}
          sx={{
            display: { xs: 'block', sm: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth, background: 'rgba(210,214,236,1)',borderRight: "rgba(0, 0, 0, 0.12) !important"  },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth, background: 'rgba(210,214,236,1)' },
          }}
          open
        >
          <div className='company-info-block'>
          {userInfo.companyName ? <><ApartmentIcon /><p>{userInfo.companyName}</p></>:<p>Admin panel</p>}
          </div>
          {drawer}
          <AccountInfo userInfo={userInfo} />
        </Drawer>
      </Box>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3, width: { sm: `calc(100% - ${drawerWidth}px)` } }}
      >
          <Toolbar />
          {props.children}
      </Box>
    </Box>
  );
}

export default memo(SideNav);