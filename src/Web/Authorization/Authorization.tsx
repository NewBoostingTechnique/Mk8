import HttpStatusCode from '../Http/HttpStatusCode.js';
import useAuthorizationClient from './AuthorizationClient.js';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function Authorization() {
  const authorizationClient = useAuthorizationClient();
  const navigate = useNavigate();

  authorizationClient.postAsync().then(async response => {

    if (response.status == HttpStatusCode.Unauthorized) {
      window.location.href = await response.text();
    }
    else {
      navigate('/');
    }
  });

  return (
    <Typography variant='h2'>Authorizing...</Typography>
  );
}
