import Layout from '../App/Layout';
import ApiError from './ApiError';
import { Typography } from '@mui/material';
import * as React from 'react';
import { useRouteError } from 'react-router-dom';
import { ReactNode } from 'react';

export default function ErrorBoundary({ children }: Readonly<{ children?: ReactNode }>) {
  let [error, setError] = React.useState<Error>();
  const routeError = useRouteError() as Error;

  const promiseRejectionHandler = React.useCallback((event: PromiseRejectionEvent) => {
    setError(event.reason);
  }, []);

  React.useEffect(() => {
    window.addEventListener("unhandledrejection", promiseRejectionHandler);

    return () => {
      window.removeEventListener("unhandledrejection", promiseRejectionHandler);
    };
  }, []);

  error = error ?? routeError;

  if (!error)
    return (children);

  return (
    <Layout authorization={false}>
      {
        error instanceof ApiError
          ? <>
            <Typography variant='h2'>{error.message}</Typography>
            <Typography variant='h3'>Trace Identifier</Typography>
            <Typography variant='body2'>{error.traceId}</Typography>
          </>
          : <Typography variant='h2'>An error occurred</Typography>
      }
    </Layout>
  );
}
