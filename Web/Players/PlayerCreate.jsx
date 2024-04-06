import { Button, MenuItem, Select, TextField } from '@mui/material';
import { useLoaderData, useNavigate } from 'react-router-dom';
import * as React from 'react';
import usePlayerClient from './PlayerClient.js';
import useRegionClient from '../Locations/Regions/RegionClient.js';

export default function PlayerCreate() {
  const loaderData = useLoaderData();
  const playerClient = usePlayerClient();
  const regionClient = useRegionClient();
  const navigate = useNavigate();

  //TODO: Default select boxes to placeholder.
  //TODO: Style this form.

  const [country, setCountry] = React.useState(loaderData.countries.find(_ => _)?.name);
  const [proof, setProof] = React.useState(loaderData.proofTypes.find(_ => _)?.id);
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

  return (
    <form onSubmit={onSubmitAsync}>
      <TextField name="name" label="Name" required />
      <Select name="countryName" label="Country" value={country} onChange={onCountryChange} required>
        {loaderData.countries.map(country =>
          <MenuItem key={country.name} value={country.name}>{country.name}</MenuItem>
        )}
      </Select>
      <Select name="regionName" label="Town/Region" value={region} onChange={onRegionChange} required>
        {regions.map(region =>
          <MenuItem key={region.name} value={region.name}>{region.name}</MenuItem>
        )}
      </Select>
      <Select name="proofTypeDescription" label="Proof" value={proof} onChange={onProofChange} required>
        {loaderData.proofTypes.map(proofType =>
          <MenuItem key={proofType.description} value={proofType.description}>{proofType.description}</MenuItem>
        )}
      </Select>
      <Button type="submit" variant="contained">Submit</Button>
    </form>
  );
}
