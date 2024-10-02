import Layout from './Layout.jsx';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { lazy } from 'react';
import { Outlet, useLoaderData } from 'react-router-dom';

const ErrorBoundary = lazy(() => import('../Errors/ErrorBoundary.jsx'));

export default function App() {
  const loaderData = useLoaderData();

  return (
    <ErrorBoundary>
      <LocalizationProvider adapterLocale={loaderData.localeName} dateAdapter={AdapterDayjs}>
        <Layout authorization={loaderData.authorization}>
          <Outlet />
        </Layout>
      </LocalizationProvider>
    </ErrorBoundary >
  );
}
