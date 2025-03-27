import usePlayerClient from '../PlayerClient.js';
import {
  Button,
  Paper,
  Stack,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Typography
} from '@mui/material';
import dayjs from 'dayjs';
import { Link, useLoaderData, useNavigate } from "react-router-dom";
import Player from '../Player.js';

export default function Index() {
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
