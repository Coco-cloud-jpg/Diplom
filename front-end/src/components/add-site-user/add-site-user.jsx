import {memo, useCallback, useEffect, useState} from 'react';
import { Button, CircularProgress, FormControl, InputLabel, LinearProgress, MenuItem, Modal, Select } from '@mui/material';
import { Box } from '@mui/system';
import "./add-site-user.css";
import { get, post, put } from '../../helpers/axiosHelper';
import { identityApiUrl, recorderApiUrl } from '../../constants';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    textAlign: "center",
    background: "#fff",
    boxShadow: 24,
};

const AddSiteUser = ({opened, close, type, data}) => {
    const [isSuccessRegistration, setIsSuccessRegistration] = useState(false);
    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({});
    const [roles, setRoles] = useState([]);
    
    useEffect(() => {
        async function getRoles() {
            setRoles((await get(`${identityApiUrl}/api/users/roles`)).data);
            console.log(data);
            if (type === 2) {
                const roleId = roles.filter(item => item.name === data.role)[0].id;
                console.log(roleId);
                setFormData({...data, ...{role: roleId}});
            }

        }
        
        getRoles();
    }, [type, data])

    const handleChange = (e) => {
      let { name, value } = e.target;
      if (name === undefined)
          name = "role";
  
      console.log(name, value);
      setFormData((prevData) => ({
          ...prevData,
          [name]: value,
        }));
    };

    const submit = async (e) => {
        if (!formData.email || !formData.firstName || !formData.lastName || !formData.role)
            return;

        e.preventDefault();
        setLoading(true);

        if (type === 2) {
            const data = await put(`${identityApiUrl}/api/users/${formData.id}`, formData);
            close();
        }
        else {
            const data = await post(`${identityApiUrl}/api/users`, formData);
            if (data.status) {
                setIsSuccessRegistration(true);
            }
        }

        setLoading(false);
    }

    const closeAll = () => {
        close();
        setIsSuccessRegistration(false);
        setFormData({});
    }

    return <>
        <Modal open={opened} onClose={() => closeAll()} sx={{width: "100%"}} className="add-user">
            <Box sx={style}>
            {isSuccessRegistration ?
                <div className='success-registration-wrapper'>
                    Email was sent to mailbox.<br/>Follow it to finish registration.
                </div>
            :
                <form>
                    <h2>User registration</h2>
                    <fieldset>
                        <ul>
                            <li>
                                <label htmlFor="firstName">First Name</label>
                                <input type="text" 
                                    id="firstName" 
                                    name="firstName" 
                                    value={formData.firstName ?? ""} 
                                    onChange={handleChange} 
                                    required 
                                    minLength="3"/>
                            </li>
                            <li>
                                <label htmlFor="lastName">Last Name</label>
                                <input type="text" 
                                    id="lastName" 
                                    name="lastName"
                                    value={formData.lastName ?? ""}
                                    onChange={handleChange} 
                                    required 
                                    minLength="3"/>
                            </li>
                            <li>
                                <label htmlFor="email">Email</label>
                                <input type="email" 
                                    id="email"
                                    name="email"
                                    value={formData.email ?? ""}
                                    onChange={handleChange}
                                    required 
                                    minLength="3"/>
                            </li>
                        </ul>
                        <FormControl sx={{ m: 1, width: "312px", '&.Mui-focused': {color: "#0F2E2F"} }} size="small" required>
                            <InputLabel id="role-label">Role</InputLabel>
                            <Select
                                labelId="role-label"
                                id="role"
                                label="Roles"
                                value={formData["role"] ?? ""}
                                onChange={handleChange}
                            >
                                {roles.map(item => <MenuItem sx={{color: "#000 !important", '&.Mui-hover': {background: "rgba(15, 46, 47, 0.2) "}, '&.Mui-selected': {background: "rgba(15, 46, 47, 0.2)"},}} 
                                    key={item.id} value={item.id}>{item.name}</MenuItem>)}
                                </Select>
                        </FormControl>
                    </fieldset>
                    <button className="primaryButton" onClick={submit}>Submit</button>
                </form>
            }
            <div className='progress'>
             {loading && <LinearProgress />}
            </div>
            </Box>
        </Modal>
    </>
}

export default memo(AddSiteUser);