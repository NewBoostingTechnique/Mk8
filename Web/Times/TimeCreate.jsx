import useTimeClient from './TimeClient.js';
import { Button, MenuItem, Stack, TextField, Typography } from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import dayjs from 'dayjs';
import * as React from 'react';
import InputMask from "react-input-mask";
import { useLoaderData, useNavigate } from 'react-router-dom';

export default function TimeCreate() {
  const loaderData = useLoaderData();
  const navigate = useNavigate();
  const timeClient = useTimeClient();

  const [courseName, setCourseName] = React.useState(loaderData.courseName);
  const [date, setDate] = React.useState(new Date());
  const [playerName, setPlayerName] = React.useState(loaderData.playerName);
  const [timeSpan, setTimeSpan] = React.useState();

  const onCourseChange = (event) => {
    setCourseName(event.target.value);
  };

  const onDateChange = (event) => {
    setDate(event.target.value);
  };

  const onPlayerChange = (event) => {
    setPlayerName(event.target.value);
  };

  const onTimeChange = (event) => {
    setTimeSpan(event.target.value);
  };

  async function onSubmitAsync(e) {
    e.preventDefault();
    let time = Object.fromEntries(new FormData(e.currentTarget).entries());
    time.date = dayjs(time.date, loaderData.localeName).toISOString().split('T')[0];
    time.span = `00:${time.span.replace(/'/g, ':').replace(/"/g, '.')}`;
    await timeClient.insertAsync(time);
    navigate(`/player/detail/${playerName}`);
  }

  // TODO Something better than always loading all players before this page is shown

  return (
    <Stack component="form" onSubmit={onSubmitAsync} sx={{ gap: 3, m: "auto", width: "75%" }}>
      <Typography variant='h3'>Create Time</Typography>
      <TextField autoFocus name="playerName" label="Player" onChange={onPlayerChange} required select value={playerName}>
        {loaderData.players.map(player =>
          <MenuItem key={player.name} value={player.name}>{player.name}</MenuItem>
        )}
      </TextField>
      <TextField name="courseName" label="Course" onChange={onCourseChange} required select value={courseName}>
        {loaderData.courses.map(course =>
          <MenuItem key={course.name} value={course.name}>{course.name}</MenuItem>
        )}
      </TextField>
      {/* TODO Required doesn't do anything? -> Form validation */}
      <InputMask mask="9'99&quot;999" value={timeSpan} disabled={false} maskChar="-" onChange={onTimeChange} required>
        {() => <TextField label="Time" name="span" />}
      </InputMask>
      <DatePicker label="Date" name="date" value={dayjs(date)} onChange={onDateChange} />
      <Button type="submit" variant="contained">Submit</Button>
    </Stack>
  );
}
