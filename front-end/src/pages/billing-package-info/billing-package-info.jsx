import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { memo } from "react";
import "./billing-package-info.css";
import BillingPackageInfoEdit from "./edit/billing-package-info-edit";
import BillingPackageInfoCompanies from "./companies-list/billing-package-info-companies";

const BillingPackageInfo = ({open, handleClose, row, reloadGrid}) => {
    console.log(row);
    return <div className="wrapper">
        <Dialog open={open} onClose={handleClose} className="dialog" fullWidth maxWidth="md">
            <DialogContent>
                <div className="content-wrapper">
                    <div className="left-side">
                        <h2>Package Info</h2>
                        <BillingPackageInfoEdit row={row} close={handleClose} reloadGrid={reloadGrid} isEdit={true}/>
                    </div>
                    <div className="left-side">
                        <h2>Companies with package</h2>
                        <BillingPackageInfoCompanies id={row.id} />
                    </div>
                </div>
            </DialogContent>
        </Dialog>
        </div>;
}

export default memo(BillingPackageInfo);