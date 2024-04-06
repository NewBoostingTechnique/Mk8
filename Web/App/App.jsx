import ErrorBoundary from '../Errors/ErrorBoundary.jsx';
import Layout from './Layout.jsx';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

import { Outlet, useLoaderData } from 'react-router-dom'

export default function App() {

  const localeName = useLoaderData();

  return (
    <ErrorBoundary>
      <LocalizationProvider adapterLocale={localeName} dateAdapter={AdapterDayjs}>
        <Layout>
          <Outlet />
        </Layout>
      </LocalizationProvider>
    </ErrorBoundary >
  );
}