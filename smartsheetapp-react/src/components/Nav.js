import React from 'react';
import { useHistory } from 'react-router-dom';
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
}));

const MAIN_STYLE = {
  height: '100%',
  display: 'flex',
  alignContent: 'center',
  backgroundColor: '#2D3436'
}

const TOOLBAR_STYLE = {
  minHeight: '100%'
}

export default function Nav() {

  let history = useHistory();
  const classes = useStyles();

  return (
    <>
      <AppBar style={MAIN_STYLE} position="static">
        <Toolbar style={TOOLBAR_STYLE}>
          <Typography variant="h6" className={classes.title} onClick = {() => history.push({ pathname: '/' })}>
            Smartsheet App
          </Typography>
          <Button color="inherit" onClick = {() => history.push({ pathname: '/Events' })}>
            Events
          </Button>
          <Button color="inherit" onClick = {() => history.push({ pathname: '/ManageWebhooks' })}>
            Manage Webhooks
          </Button>
        </Toolbar>
      </AppBar>
    </>
  );  
}