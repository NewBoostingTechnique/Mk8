import useSyncClient from './SyncClient.js';
import { Button, Typography, Stack } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function SyncCreate() {
  const syncClient = useSyncClient();
  const navigate = useNavigate();

  async function onSubmitAsync(e) {
    e.preventDefault();
    let sync = {};
    sync = await syncClient.insertAsync(sync);
    navigate(`/syncs/detail/${sync.id}`);
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: { xs: 2, md: 3 } }} >
      <Typography variant='h2'>Create Sync</Typography>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}
