import { Button, Card, Grid, Typography } from '@mui/material';
import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/duration';
import DOMPurify from 'dompurify';
import { useLoaderData, useNavigate } from 'react-router-dom';
import useNewClient from './NewClient';

dayjs.extend(localizedFormat);

export function Index() {
  const loaderData = useLoaderData();
  const navigate = useNavigate();
  const newClient = useNewClient();

  async function migrateAsync() {
    const migration = await newClient.migrateAsync();
    navigate(`/migrations/detail/${migration.id}/`);
  }

  return (
    <Grid container spacing={{ xs: 2, md: 3 }}>
      <Grid item xs={12}>
        <Typography variant='h2'>News</Typography>
        {
          loaderData.authorization
            ? <Grid item xs={12}>
              <Button role='button' variant="contained" onClick={migrateAsync}>Migrate</Button>
            </Grid>
            : null
        }
      </Grid>
      {
        loaderData.news.map($new => (
          <Grid item key={$new.title}>
            <Card sx={{ padding: { xs: 1, md: 3 } }}>
              <Typography variant='h4'>{$new.title}</Typography>
              <Typography variant='p'>By {$new.authorName} on {dayjs($new.date).format('LL')}.</Typography>
              <div dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize($new.body, { USE_PROFILES: { html: true } }) }} />
            </Card>
          </Grid>
        ))
      }
    </Grid >
  );
}
