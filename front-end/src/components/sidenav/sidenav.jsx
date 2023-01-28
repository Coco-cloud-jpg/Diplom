import {memo, useState} from 'react';
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

const navData = [
        {
            id: 0,
            icon: <HomeIcon style={{color: "rgba(118,181,238,1)", background: "#fff"}}/>,
            text: "Home",
            link: "/home"
        },
        {
            id: 1,
            icon: <ScreenshotMonitorIcon  style={{color: "rgba(118,181,238,1)", background: "#fff"}}/>,
            text: "Recorders",
            link: "/recorders"
        }
    ]

const drawerWidth = 240;

const SideNav = (props) => {
  const { window } = props;
  const [mobileOpen, setMobileOpen] = useState(false);
  const location = useLocation();
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const navigate = useNavigate();

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const drawer = (
    <div>
      <Toolbar />
      <Divider />
      <List>
        {navData.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton onClick={() => {navigate(`/${item.text.toLowerCase()}`)}}>
              <ListItemIcon>
                {item.icon}
              </ListItemIcon>
              <ListItemText primaryTypographyProps={{ style: {color: "#fff"} }} primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      <Divider />
      <List>
        <ListItem disablePadding>
            <ListItemButton onClick={() => {setTokens(null); navigate("/login")}}>
              <ListItemIcon >
                <LogoutIcon style={{color: "rgba(118,181,238,1)", background: "#fff"}}/>
              </ListItemIcon>
              <ListItemText  primaryTypographyProps={{ style: {color: "#fff"} }} primary={"Log out"} />
            </ListItemButton>
          </ListItem>
      </List>
    </div>
  );

  const container = window !== undefined ? () => window().document.body : undefined;

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar
        position="fixed"
        sx={{
          width: { sm: `calc(100% - ${drawerWidth}px)` },
          ml: { sm: `${drawerWidth}px` },
          background: "rgba(118,181,238,1)",
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
          <Typography variant="h6" noWrap component="div" sx={{ textTransform: "capitalize" }}>
            {location.pathname.replace("/","").replaceAll("/", ":")}
          </Typography>
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
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth, background: 'linear-gradient(0deg, rgba(85,136,228,1) 0%, rgba(118,181,238,1) 37%)'  },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth, background: 'linear-gradient(0deg, rgba(85,136,228,1) 0%, rgba(118,181,238,1) 37%)' },
          }}
          open
        >
          {drawer}
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

/*function Sidenav(props) {
  const [tokens, setTokens] = useLocalStorage("tokens", null);
  const navigate = useNavigate();

  const drawer = (
    <div>
      <Toolbar />
      <Divider />
      <List>
        {navData.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton onClick={() => {navigate(`/${item.text.toLowerCase()}`)}}>
              <ListItemIcon>
                {item.icon}
              </ListItemIcon>
              <ListItemText primaryTypographyProps={{ style: {color: "#fff"} }} primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      <Divider />
      <List>
        <ListItem disablePadding>
            <ListItemButton onClick={() => {setTokens(null); navigate("/login")}}>
              <ListItemIcon >
                <LogoutIcon style={{color: "rgba(118,181,238,1)", background: "#fff"}}/>
              </ListItemIcon>
              <ListItemText  primaryTypographyProps={{ style: {color: "#fff"} }} primary={"Log out"} />
            </ListItemButton>
          </ListItem>
      </List>
    </div>
  );

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
        aria-label="mailbox folders"
      >
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth, background: 'linear-gradient(0deg, rgba(85,136,228,1) 0%, rgba(118,181,238,1) 37%)' },
          }}
          open
        >
          {drawer}
        </Drawer>
      </Box>
    </Box>
  );
}

export default Sidenav;
/*
const SideNav = () => {
    const [open, setopen] = useState(true);
    const toggleOpen = () => {
            setopen(!open)
        }

    return <div className={open? "sidenav":"sidenavClosed"}>
        <div className="buttonWrapper">
            <button className="menuBtn" onClick={toggleOpen}>
                {open? <KeyboardDoubleArrowLeftIcon />: <KeyboardDoubleArrowRightIcon />}
            </button>
        </div>
        {navData.map(item =>{
            return <NavLink key={item.id} className='sideitem' to={item.link}>
                      
                       <span className={open? "linkText": "linkTextClosed"}>{item.icon}{item.text}</span>
                   </NavLink>
         })}
    </div>
}
 */
export default memo(SideNav);