import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";
import { memo } from "react";
import BillingPackageInfoEdit from "../edit/billing-package-info-edit";
import "./billing-package-add.css"

const BillingPackageAdd = ({open, handleClose, reloadGrid}) => {
    return <div className="wrapper">
        <Dialog open={open} onClose={handleClose} className="dialog" fullWidth maxWidth="sm">
            <DialogContent>
                <div className="content-wrapper-add">
                    <h2>Package Creation</h2>
                    <BillingPackageInfoEdit row={{}} close={handleClose} reloadGrid={reloadGrid} isEdit={false}/>
                </div>
            </DialogContent>
        </Dialog>
        </div>;
}

export default memo(BillingPackageAdd);