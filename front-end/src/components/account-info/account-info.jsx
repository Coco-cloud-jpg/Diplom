import { List, ListItem, ListItemButton, ListItemIcon, ListItemText, Popover } from "@mui/material";
import { memo, useState } from "react";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import "./account-info.css";
import ApartmentIcon from '@mui/icons-material/Apartment';

const AccountInfo = ({userInfo}) => {
    const [anchorEl, setAnchorEl] = useState(null);

    const handleClick = (e) => {
      setAnchorEl(e.currentTarget);
    };
  
    const handleClose = () => {
      setAnchorEl(null);
    };
  
    const open = Boolean(anchorEl);
    const id = open ? 'simple-popover' : undefined;

    return <>
        <Popover 
            id={id}
            open={open}
            anchorEl={anchorEl}
            onClose={handleClose}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'center',
            }}
            transformOrigin={{
              vertical: 'bottom',
              horizontal: 'center',
            }}
          >
            <div className="account-info">
                <div className="account-icon">
                    <AccountCircleIcon style={{color: "#1976D2"}}/>
                </div>
                <p className="full-name">{userInfo.fullName}</p>
                <p className="email-part">{userInfo.email}</p>
                <p  className="role-part">{userInfo.roleName}</p>
            </div>
          </Popover>
        <List sx={{background: "#1976D2"}}>
          <ListItem disablePadding>
              <ListItemButton aria-describedby={id} onClick={handleClick}>
                <ListItemIcon >
                  <AccountCircleIcon style={{color: "#fff", fontSize: "30px"}}/>
                </ListItemIcon>
                <div className='online-status'>
                  <div></div>
                </div>
              </ListItemButton>
            </ListItem>
        </List>
    </>
}

export default memo(AccountInfo);