import {
  Button,
  Paper,
  Stack,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Typography
} from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useLoaderData, useNavigate } from 'react-router-dom';

import useMigrationClient from './MigrationClient.js';
import Migration from './Migration.js';


export function Create() {
  const migrationClient = useMigrationClient();
  const navigate = useNavigate();

  async function onSubmitAsync(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const migration = await migrationClient.createAsync();
    navigate(`/migrations/detail/${migration.id}`);
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: { xs: 2, md: 3 } }} >
      <Typography variant='h2'>Create Migration</Typography>
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}

export function Detail() {

  const loaderData = useLoaderData();
  const migrationClient = useMigrationClient();
  const [migration, setMigration] = useState(loaderData.migration);

  useEffect(() => {
    const interval = setInterval(async () => {
      const migration = await migrationClient.detailAsync(loaderData.migration.id);
      setMigration(migration);
      if (migration.progress === 100 || migration.endTime || migration.error)
        clearInterval(interval);
    }, 1000);

    return () => clearInterval(interval);
  }, []);


  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Migration Detail</Typography>
      <Typography variant='h3'>Description</Typography>
      <Typography variant="body1">{migration.description}</Typography>
      <Typography variant='h3'>Progress</Typography>
      <Typography variant="body1">{migration.progress}%</Typography>
      {
        migration.error ? <>
          <Typography variant='h3'>Error</Typography>
          <Typography variant="body2">{migration.error}</Typography>
        </> : null
      }
      <Typography variant='h3'>Start Time</Typography>
      <Typography variant="body1">{migration.startTime}</Typography>
      <Typography variant='h3'>End Time</Typography>
      <Typography variant="body1">{migration.endTime}</Typography>
    </Stack>
  );
}

export function Index() {
  const loaderData = useLoaderData();
  const migrationClient = useMigrationClient();
  const navigate = useNavigate();

  const [migrations, setMigrations] = useState(loaderData.migrations);

  async function loadMore() {
    const last = migrations[migrations.length - 1];
    setMigrations(migrations.concat(await migrationClient.indexAsync({
      Id: last.id,
      StartTime: last.startTime,
    })));
  }

  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Migrations</Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Start</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Progress</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {migrations.map((migration: Migration) => (
              <TableRow key={migration.id} onClick={() => navigate(`/migrations/detail/${migration.id}`)}>
                <TableCell>{migration.startTime}</TableCell>
                <TableCell>{migration.description}</TableCell>
                <TableCell>{migration.progress}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <Button onClick={loadMore} variant="contained">Load More</Button>
    </Stack>
  );
}
