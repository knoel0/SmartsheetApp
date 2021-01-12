import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Nav from './components/Nav';
import Home from './pages/Home';
import Events from './pages/Events';
import ManageWebhooks from './pages/ManageWebhooks';

const APP_STYLE = {
  height: '100%',
  width: '100%',
  display: 'grid',
  gridTemplateColumns: 'repeat(8, 1fr)',
  gridTemplateRows: 'repeat(8, 1fr)',
  gridColumnGap: '0px',
  gridRowGap: '0px',
  backgroundColor: '#636E72'
}

const NAV_STYLE = {
  gridArea: '1 / 1 / 2 / 9'
}

const MAIN_STYLE = {
  gridArea: '2 / 1 / 9 / 9',
  display: 'flex',
  backgroundColor: '#B2BEC3',
  justifyContent: 'center',
  alignContent: 'center',
  margin: '40px'
}

export default function App() {

  return (
      <div style={APP_STYLE}>
        <div style={NAV_STYLE}>
          <Nav />
        </div>
        <div style={MAIN_STYLE}>
          <Switch>
            <Route exact path = '/' component={Home} />
            <Route exact path = '/Events' component={Events} />
            <Route exact path = '/ManageWebhooks' component={ManageWebhooks} />
          </Switch>
        </div>
    </div>
  );  
}