import HttpStatusCode from '../Http/HttpStatusCode.js';
import useAuthorizationClient from './AuthorizationClient.js';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function Authorization() {
  const authorizationClient = useAuthorizationClient();
  const navigate = useNavigate();

  authorizationClient.postAsync().then(async response => {

    if (response.status == HttpStatusCode.Unauthorized) {
      window.location = await response.text();
    }
    else {
      navigate('/');
    }
  });

  // TODO: Better indicator.
  // TODO: Update to reflect when authenticating vs authorizing.
  return (
    <Typography variant='h3'>Authorizing...</Typography>
  );
}