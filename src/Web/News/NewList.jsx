import { Card, Grid, Typography } from '@mui/material';
import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/duration';
import DOMPurify from 'dompurify';
import { useLoaderData } from "react-router-dom";

dayjs.extend(localizedFormat);

export default function NewsList() {
  const loaderData = useLoaderData();

  return (
    <Grid container spacing={{ xs: 2, md: 3 }}>
      <Grid item xs={12}>
        <Typography variant='h2'>News</Typography>
      </Grid>
      {loaderData.map(news => (
        <Grid item key={news.title}>
          <Card sx={{ padding: { xs: 1, md: 3 } }}>
            <Typography variant='h4'>{news.title}</Typography>
            <Typography variant='p'>By {news.authorName} on {dayjs(news.date).format('LL')}.</Typography>
            <div dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(news.body, { USE_PROFILES: { html: true } }) }} />
          </Card>
        </Grid>
      ))}
    </Grid >
  );
}
