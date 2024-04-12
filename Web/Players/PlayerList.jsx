import ProofType from '../ProofTypes/ProofType.js';
import CheckIcon from '@mui/icons-material/Check';
import QuestionMarkIcon from '@mui/icons-material/QuestionMark';
import {
  Button,
  Paper,
  Stack,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Typography
} from '@mui/material';
import dayjs from 'dayjs';
import { Link, useLoaderData, useNavigate } from "react-router-dom";

export default function PlayerList() {
  const loaderData = useLoaderData();
  const navigate = useNavigate();

  // TODO: Move 'create' button to app bar.
  // TODO: Sorting.

  function getActive(player) {
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

  function getLocation(player) {
    let location = player.regionName ? `${player.regionName}, ` : '';
    location += player.countryName;
    return location;
  }

  function getProof(player) {
    switch (player.proofTypeDescription) {
      case ProofType.NoProof.description:
        return <></>;
      case ProofType.Screenshot.description:
      case ProofType.TimeScroll.description:
        return <CheckIcon style={{ color: 'yellow' }} />;
      case ProofType.Video.description:
      case ProofType.Ghost.description:
        return <CheckIcon style={{ color: 'green' }} />;
      default:
        return <QuestionMarkIcon />;
    }
  }

  return (
    <Stack sx={{ gap: 3 }}>
      <Typography variant='h3'>Players</Typography>
      {
        loaderData.authorization === true
          ? <Button component={Link} to={'/player/create/'} role='button' variant="contained">Create</Button>
          : null
      }
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Location</TableCell>
              <TableCell>Proof</TableCell>
              <TableCell>Active</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loaderData.players.map((player) => (
              <TableRow key={player.name} onClick={() => navigate(`/player/detail/${player.name}`)}>
                <TableCell>{player.name}</TableCell>
                <TableCell>{getLocation(player)}</TableCell>
                <TableCell>{getProof(player)}</TableCell>
                <TableCell>{getActive(player)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Stack>
  );
}
