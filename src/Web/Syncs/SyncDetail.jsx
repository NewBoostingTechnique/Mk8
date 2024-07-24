import {
  List, ListItem, ListItemText,
  Stack,
  Typography
} from '@mui/material';
import * as React from 'react';
import { useLoaderData } from "react-router-dom";

export default function PlayerDetail() {
  const loaderData = useLoaderData();

  return (
    <Stack sx={{ gap: 3 }}>
      <Typography variant='h3'>Sync Detail</Typography>
      <List>
        <ListItem>
          <ListItemText>Started</ListItemText>
          {/* TODO: Display date times in readable and local format. */}
          <ListItemText>{loaderData.sync.startTime}</ListItemText>
        </ListItem>
        <ListItem>
          <ListItemText>Ended</ListItemText>
          <ListItemText>{loaderData.sync.endTime}</ListItemText>
        </ListItem>
      </List >
    </Stack>
  );
}
