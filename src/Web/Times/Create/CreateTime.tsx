import useTimeClient from '../TimeClient.js';
import Time from '../Time.ts';
import { Button, MenuItem, Stack, TextField, Typography } from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import dayjs, { Dayjs } from 'dayjs';
import * as React from 'react';
import InputMask from "react-input-mask";
import { useLoaderData, useNavigate } from 'react-router-dom';
import Player from '../../Players/Player.ts';
import Course from '../../Courses/Course.ts';

export default function TimeCreate() {
  const loaderData = useLoaderData();
  const navigate = useNavigate();
  const timeClient = useTimeClient();

  const [courseName, setCourseName] = React.useState(loaderData.courseName);
  const [date, setDate] = React.useState<Dayjs | null>(new Dayjs());
  const [playerName, setPlayerName] = React.useState(loaderData.playerName);
  const [timeSpan, setTimeSpan] = React.useState<string>();

  const onCourseChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setCourseName(event.target.value);
  };

  const onDateChange = (value: Dayjs | null) => {
    setDate(value);
  };

  const onPlayerChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setPlayerName(event.target.value);
  };

  const onTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setTimeSpan(event.target.value);
  };

  async function onSubmitAsync(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const elements = e.currentTarget.elements;

    const time = new Time(
      dayjs((elements.namedItem('data') as HTMLInputElement).value, loaderData.localeName).toISOString().split('T')[0],
      `00:${(elements.namedItem('span') as HTMLInputElement).value.replace(/'/g, ':').replace(/"/g, '.')} `,
      (elements.namedItem('courseName') as HTMLInputElement).value,
      (elements.namedItem('playerName') as HTMLInputElement).value
    );
    await timeClient.createAsync(time);
    navigate(`/ players / detail / ${playerName} `);
  }

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant='h2'>Create Time</Typography>
      <TextField autoFocus name="playerName" label="Player" onChange={onPlayerChange} required select value={playerName}>
        {loaderData.players.map((player: Player) =>
          <MenuItem key={player.name} value={player.name}>{player.name}</MenuItem>
        )}
      </TextField>
      <TextField name="courseName" label="Course" onChange={onCourseChange} required select value={courseName}>
        {loaderData.courses.map((course: Course) =>
          <MenuItem key={course.name} value={course.name}>{course.name}</MenuItem>
        )}
      </TextField>
      <InputMask mask="9'99&quot;999" value={timeSpan} disabled={false} maskChar="-" onChange={onTimeChange} required>
        {() => <TextField label="Time" name="span" />}
      </InputMask>
      <DatePicker label="Date" name="date" value={dayjs(date)} onChange={onDateChange} />
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}
