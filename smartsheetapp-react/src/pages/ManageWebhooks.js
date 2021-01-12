import React, { useState, useEffect } from 'react';
import { Switch, FormControl, FormControlLabel, Button } from '@material-ui/core';
import { useHistory } from 'react-router-dom';

const FORM_STYLE = {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignContent: 'center'
}

const FORMCONTROL_STYLE = {
    padding: '40px'
}

export default function ManageWebhooks() {

    let history = useHistory();
    const [webhooks, setWebhooks] = useState([])
    
    const getWebhooks = () =>
        fetch("https://smartsheetappnetcore.azurewebsites.net/ManageWebhooks")
            .then((res) => res.json())

    const submitHandler = () => {
        
        var message = {
            method: 'POST',
            headers: {},
            body: JSON.stringify(webhooks)
        };

        fetch("https://smartsheetappnetcore.azurewebsites.net/ManageWebhooks/ToggleWebhookEnabled", message)
            .then(res => res.json())
            .then(data => {
                console.log('Success:', data)
            })
            .catch((error) => {
                console.log('Error:', error);
            });
        
        history.push({ pathname: '/' })
    }

    const toggleHandler = (e, index) => {
        let whs = [...webhooks];
        let wh = {...whs[index]};
        wh.enabled = e.currentTarget.checked;
        whs[index] = wh;
        setWebhooks(whs);
    };

    useEffect(() => {
        getWebhooks().then((data) => setWebhooks(data));
    }, [])

    const toggleSwitches = []

    for (let i = 0; i < webhooks.length; i++) {
        toggleSwitches.push(<FormControlLabel
            control={<Switch
              checked={webhooks[i].enabled}
              onChange={e => {toggleHandler(e, i);}}
            />}
            label={webhooks[i].sheetName}
            labelPlacement="start"

        />);
    }

    return (
        <form style={FORM_STYLE} onSubmit={submitHandler}>
            <FormControl style={FORMCONTROL_STYLE} component="fieldset">
                {toggleSwitches}
            </FormControl>
            <Button
                variant="contained"
                color="#f50057"
                type="submit"
            >Submit
            </Button>
        </form>
    )
}