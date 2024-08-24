import {
  Stack,
  Typography
} from '@mui/material';
import * as React from 'react';
import { useLoaderData } from "react-router-dom";

export default function PlayerDetail() {
  const loaderData = useLoaderData();

  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Import Detail</Typography>
      <Typography variant='h3'>Started</Typography>
      <Typography variant="body1">{loaderData.import.startTime}</Typography>
      <Typography variant='h3'>Ended</Typography>
      <Typography variant="body1">{loaderData.import.endTime}</Typography>
      {
        loaderData.import.error ? <>
          <Typography variant='h3'>Error</Typography>
          <Typography variant="body2">{loaderData.import.error}</Typography>
        </> : null
      }
    </Stack>
  );
}
