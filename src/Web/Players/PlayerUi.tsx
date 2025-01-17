import usePlayerClient from './PlayerClient.js';
import useRegionClient from '../Regions/RegionClient.js';
import {
  Button,
  Card,
  Dialog, DialogActions, DialogTitle,
  MenuItem,
  Paper,
  Stack,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  TextField,
  Typography
} from '@mui/material';
import dayjs from 'dayjs';
import duration from 'dayjs/plugin/duration';
import PropTypes from 'prop-types';
import { Link, useLoaderData, useNavigate } from "react-router-dom";
import * as React from 'react';
import Player from './Player.js';
import Time from '../Times/Time.js';
import Region from '../Regions/Region.js';
import Country from '../Countries/Country.js';

export function Create() {
  const loaderData = useLoaderData();
  const playerClient = usePlayerClient();
  const regionClient = useRegionClient();
  const navigate = useNavigate();

  const [countryName, setCountryName] = React.useState<string>();
  const [regionName, setRegionName] = React.useState<string>();
  const [regions, setRegions] = React.useState([]);

  const onCountryChange = async (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setCountryName(event.target.value);
    setRegions(await regionClient.listAsync(event.target.value));
  };

  const onRegionChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setRegionName(event.target.value);
  }

  async function onSubmitAsync(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const elements = e.currentTarget.elements;
    const player = new Player(
      (elements.namedItem('name') as HTMLInputElement).value,
      null,
      (elements.namedItem('countryName') as HTMLInputElement).value,
      (elements.namedItem('regionName') as HTMLInputElement).value
    );
    await playerClient.createAsync(player);
    navigate(`/players/detail/${player.name}`);
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Create Player</Typography>
      <TextField autoFocus name="name" label="Name" required />
      <TextField name="countryName" label="Country" onChange={onCountryChange} required select value={countryName}>
        {loaderData.countries.map((country: Country) =>
          <MenuItem key={country.name} value={country.name}>{country.name}</MenuItem>
        )}
      </TextField>
      <TextField name="regionName" label="Town/Region" onChange={onRegionChange} required select value={regionName} >
        {regions.map((region: Region) =>
          <MenuItem key={region.name} value={region.name}>{region.name}</MenuItem>
        )}
      </TextField>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}

//#region Detail

export function Detail() {
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
  }).format('m\'ss"SSS');
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


//#endregion Detail

export function Index() {
  const playerClient = usePlayerClient();
  const loaderData = useLoaderData();
  const navigate = useNavigate();

  function getActive(player: Player) {
    if (!player.active)
      return 'Never';

    const today = dayjs().startOf('day');
    const active = dayjs(player.active);

    const years = today.diff(active, 'years');
    if (years > 1)
      return `${years} years`;
    if (years === 1)
      return '1 year';

    const months = today.diff(active, 'months');
    if (months > 1)
      return `${months} months`;
    if (months === 1)
      return '1 month';

    const days = today.diff(active, 'days');
    if (days > 1)
      return `${days} days`;
    if (days === 1)
      return '1 day';

    return 'Today';
  }

  function getLocation(player: Player) {
    let location = player.regionName ? `${player.regionName}, ` : '';
    location += player.countryName;
    return location;
  }

  async function migrateAsync() {
    const migration = await playerClient.migrateAsync();
    navigate(`/migrations/detail/${migration.id}`);
  }

  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Players</Typography>
      {
        loaderData.authorization === true
          ? <>
            <Button component={Link} to={'/players/create/'} role='button' variant="contained">Create</Button>
            <Button role='button' variant="contained" onClick={migrateAsync}>Migrate</Button>
          </>
          : null
      }
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Location</TableCell>
              <TableCell>Active</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loaderData.players.map((player: Player) => (
              <TableRow key={player.name} onClick={() => navigate(`/players/detail/${player.name}`)}>
                <TableCell>{player.name}</TableCell>
                <TableCell>{getLocation(player)}</TableCell>
                <TableCell>{getActive(player)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Stack>
  );
}
