import useSyncClient from './SyncClient.js';
import { Button, Typography, Stack } from '@mui/material';

export default function SyncCreate() {
  const syncClient = useSyncClient();

  async function onSubmitAsync(e) {
    e.preventDefault();
    return syncClient.insertAsync();
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: 3, m: "auto", width: "75%" }}>
      <Typography variant='h3'>Create Sync</Typography>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}