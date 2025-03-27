import usePlayerClient from '../PlayerClient.js';
import useRegionClient from '../../Regions/RegionClient.js';
import {
  Button,
  MenuItem,
  Stack,
  TextField,
  Typography
} from '@mui/material';
import { useLoaderData, useNavigate } from "react-router-dom";
import * as React from 'react';
import Player from '../Player.js';
import Region from '../../Regions/Region.js';
import Country from '../../Countries/Country.js';

export default function Create() {
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
