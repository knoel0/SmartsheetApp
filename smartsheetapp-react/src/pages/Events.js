import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Box from '@material-ui/core/Box';
import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Typography from '@material-ui/core/Typography';

const TABLECONTAINER_STYLE = {
  backgroundColor: '#B2BEC3'
}

const useRowStyles = makeStyles({
  root: {
    '& > *': {
      borderBottom: 'unset'
    },
  }
});

function Row(props) {
  
  const { row } = props;
  const [open, setOpen] = React.useState(false);
  const classes = useRowStyles();

  return (
    <React.Fragment>
      <TableRow className={classes.root}>
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell component="th" scope="row">{row.scopeObjectName}</TableCell>
        <TableCell>{row.timestamp}</TableCell>
      </TableRow>
      <TableRow>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box margin={1}>
              <Typography variant="h6" gutterBottom component="div">
                Events
              </Typography>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Object Type</TableCell>
                    <TableCell>Event Type</TableCell>
                    <TableCell>Row's First Cell</TableCell>
                    <TableCell>Column</TableCell>
                    <TableCell>Value</TableCell>
                    <TableCell>User ID</TableCell>
                    <TableCell>Timestamp</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {row.events.map((eventRow) => (
                    <TableRow key={eventRow.objectType}>
                      <TableCell component="th" scope="row">{eventRow.objectType}</TableCell>
                      <TableCell>{eventRow.eventType}</TableCell>
                      <TableCell>{eventRow.rowFirstCell}</TableCell>
                      <TableCell>{eventRow.columnName}</TableCell>
                      <TableCell>{eventRow.cellValue}</TableCell>
                      <TableCell>{eventRow.userId}</TableCell>
                      <TableCell>{eventRow.timestamp}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </React.Fragment>
  );
}

export default function Events() {

  const [events, setEvents] = useState([])
  
  const getEvents = () =>
      fetch("https://smartsheetappnetcore.azurewebsites.net/Events")
          .then((res) => res.json())

  useEffect(() => {
      getEvents().then((data) => setEvents(data));
  }, [])

  return (
      <>
      <TableContainer style={TABLECONTAINER_STYLE} component={Paper}>
          <Table aria-label="collapsible table">
              <TableHead>
                  <TableRow>
                      <TableCell/>
                      <TableCell>Sheet</TableCell>
                      <TableCell>Timestamp</TableCell>
                  </TableRow>
              </TableHead>
              <TableBody>
                  {events.map((row) => (
                      <Row key={row.scopeObjectName} row={row} />
                  ))}
              </TableBody>
          </Table>
      </TableContainer>
      </>
  )
}