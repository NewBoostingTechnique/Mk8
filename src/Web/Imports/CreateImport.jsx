import useImportClient from './ImportClient.js';
import { Button, Typography, Stack } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function CreateImport() {
  const importClient = useImportClient();
  const navigate = useNavigate();

  async function onSubmitAsync(e) {
    e.preventDefault();
    let $import = {};
    $import = await importClient.insertAsync($import);
    navigate(`/imports/detail/${$import.id}`);
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: { xs: 2, md: 3 } }} >
      <Typography variant='h2'>Create Import</Typography>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}
