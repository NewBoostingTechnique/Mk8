import { Card, Stack, Typography } from '@mui/material';
import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/duration';
import DOMPurify from 'dompurify';
import { useLoaderData } from "react-router-dom";

dayjs.extend(localizedFormat);

export default function NewsList() {
  const loaderData = useLoaderData();

  return (
    <Stack sx={{ gap: 3 }}>
      <Typography variant='h3'>News</Typography>
      {loaderData.map(news => (
        <Card key={news.title} sx={{ padding: 3 }}>
          <Typography variant='h4'>{news.title}</Typography>
          <Typography variant='p'>By {news.authorName} on {dayjs(news.date).format('LL')}.</Typography>
          <div dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(news.body, { USE_PROFILES: { html: true } }) }} />
        </Card>
      ))}
    </Stack >
  );
}
