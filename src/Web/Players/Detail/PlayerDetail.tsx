import usePlayerClient from '../PlayerClient.js';
import {
  Button,
  Card,
  Dialog, DialogActions, DialogTitle,
  Paper,
  Stack,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Typography
} from '@mui/material';
import dayjs from 'dayjs';
import duration from 'dayjs/plugin/duration';
import PropTypes from 'prop-types';
import { useLoaderData, useNavigate } from "react-router-dom";
import * as React from 'react';
import Player from '../Player.js';
import Time from '../../Times/Time.js';

export default function Detail() {
  const [isDialogOpen, setDialogOpen] = React.useState(false);
  const loaderData = useLoaderData();
  const navigate = useNavigate();
  const playerClient = usePlayerClient();

  async function deletePlayerAsync(name: string) {
    await playerClient.deleteAsync(name);
    navigate('/players/');
  }

  function closeDialog() {
    setDialogOpen(false);
  }

  function openDialog() {
    setDialogOpen(true);
  }

  if (!loaderData.player) {
    return (
      <Typography variant='h2'>Player Not Found</Typography>
    );
  }

  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>{loaderData.player.name}</Typography>
      {
        loaderData.authorization === true
          ? <>
            <Button onClick={openDialog} variant="contained">Delete</Button>
            <Dialog open={isDialogOpen}>
              <DialogTitle>Delete Player '{loaderData.player.name}'?</DialogTitle>
              <DialogActions>
                <Button onClick={closeDialog} autoFocus>No</Button>
                <Button onClick={() => deletePlayerAsync(loaderData.player.name)}>Yes</Button>
              </DialogActions>
            </Dialog>
          </>
          : null
      }
      <TableContainer component={Card}>
        <Table>
          <TableRow>
            <TableCell>Country</TableCell>
            <TableCell>{loaderData.player.countryName}</TableCell>
          </TableRow>
          <TableRow>
            <TableCell>Town/Region</TableCell>
            <TableCell>{loaderData.player.regionName}</TableCell>
          </TableRow>
          <TableRow>
            <TableCell>Proof Status</TableCell>
            <TableCell>{loaderData.player.proofTypeDescription}</TableCell>
          </TableRow>
        </Table>
      </TableContainer>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Course</TableCell>
              <TableCell>Time</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loaderData.player.times?.map((time: Time) => (
              <TimeRow authorization={loaderData.authorization} key={`${time.courseName}-${time.playerName}`} player={loaderData.player} time={time}></TimeRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Stack>
  );
}

dayjs.extend(duration);

function formatTimeSpan(timeSpan: string) {
  if (!timeSpan)
    return "-'--\"---";

  const parts = timeSpan.split(':');
  const hours = parseInt(parts[0]);
  const minutes = parseInt(parts[1]);
  const seconds = parseInt(parts[2].split('.')[0]);
  const milliseconds = parseInt(parts[2].split('.')[1]);

  return dayjs.duration({
    hours,
    minutes,
    seconds,
    milliseconds
  }).format('m:ss.SSS');
}

function TimeCells({ time }: Readonly<{ time: Time }>) {
  return (
    <>
      <TableCell>{time.courseName}</TableCell>
      <TableCell>{formatTimeSpan(time.span)}</TableCell>
    </>
  )
}

TimeCells.propTypes = {
  time: PropTypes.object.isRequired
};

function TimeRow({ authorization, player, time }: Readonly<{ authorization: boolean, player: Player, time: Time }>) {
  const navigate = useNavigate();
  if (authorization) {
    if (time.span) {
      return (
        <TableRow onClick={() => navigate(`/times/edit/${player.name}/${time.courseName}/`)}>
          <TimeCells time={time} />
        </TableRow>
      );
    } else {
      return (
        <TableRow onClick={() => navigate(`/times/create/${player.name}/${time.courseName}/`)}>
          <TimeCells time={time} />
        </TableRow>
      );
    }
  }
  else {
    return (
      <TableRow>
        <TimeCells time={time} />
      </TableRow>
    );
  }
}

TimeRow.propTypes = {
  authorization: PropTypes.object.isRequired,
  player: PropTypes.object.isRequired,
  time: PropTypes.object.isRequired
};
