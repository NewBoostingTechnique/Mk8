import Layout from '../App/Layout';
import ApiError from './ApiError';
import { Typography } from '@mui/material';
import PropTypes from 'prop-types';
import * as React from 'react';
import { useRouteError } from 'react-router-dom';

export default function ErrorBoundary({ children }) {
  let [error, setError] = React.useState();

  const promiseRejectionHandler = React.useCallback((event) => {
    setError(event.reason);
  }, []);

  React.useEffect(() => {
    window.addEventListener("unhandledrejection", promiseRejectionHandler);

    return () => {
      window.removeEventListener("unhandledrejection", promiseRejectionHandler);
    };
  }, []);

  error ??= useRouteError();

  if (!error)
    return (children);

  return (
    <Layout>
      {
        error instanceof ApiError
          ? <>
            <Typography variant='h2'>{error.message}</Typography>
            <Typography variant='h3'>Trace Identifier</Typography>
            <Typography variant='body'>{error.traceId}</Typography>
          </>
          : <Typography variant='h2'>An error occurred</Typography>
      }
    </Layout>
  );
}
ErrorBoundary.propTypes = {
  children: PropTypes.string
};
