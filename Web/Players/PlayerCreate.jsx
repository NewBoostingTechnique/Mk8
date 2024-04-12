import { Button, MenuItem, Stack, TextField, Typography } from '@mui/material';
import { useLoaderData, useNavigate } from 'react-router-dom';
import * as React from 'react';
import usePlayerClient from './PlayerClient.js';
import useRegionClient from '../Locations/Regions/RegionClient.js';

export default function PlayerCreate() {
  const loaderData = useLoaderData();
  const playerClient = usePlayerClient();
  const regionClient = useRegionClient();
  const navigate = useNavigate();

  const [country, setCountry] = React.useState();
  const [proof, setProof] = React.useState();
  const [region, setRegion] = React.useState();
  const [regions, setRegions] = React.useState([]);

  const onCountryChange = async (event) => {
    setCountry(event.target.value);
    setRegions(await regionClient.listAsync(event.target.value));
  };

  const onProofChange = (event) => {
    setProof(event.target.value);
  };

  const onRegionChange = (event) => {
    setRegion(event.target.value);
  }

  async function onSubmitAsync(e) {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);
    let player = Object.fromEntries(formData.entries());
    player = await playerClient.insertAsync(player);
    navigate(`/player/detail/${player.name}`);
  }

  // TODO: Factor styles into a new form component

  // TODO: Factory page title into Layout component.

  // TODO: Alignment of MenuItems on narrower screens -> Due to the banner style / size of parent container?

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: 3 }}>
      <Typography variant='h3'>Create Player</Typography>
      <TextField autoFocus name="name" label="Name" required />
      <TextField name="countryName" label="Country" onChange={onCountryChange} required select value={country}>
        {loaderData.countries.map(country =>
          <MenuItem key={country.name} value={country.name}>{country.name}</MenuItem>
        )}
      </TextField>
      <TextField name="regionName" label="Town/Region" onChange={onRegionChange} required select value={region} >
        {regions.map(region =>
          <MenuItem key={region.name} value={region.name}>{region.name}</MenuItem>
        )}
      </TextField>
      <TextField name="proofTypeDescription" label="Proof" onChange={onProofChange} required select value={proof}>
        {loaderData.proofTypes.map(proofType =>
          <MenuItem key={proofType.description} value={proofType.description}>{proofType.description}</MenuItem>
        )}
      </TextField>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}
