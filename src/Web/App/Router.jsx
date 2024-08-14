import Authorization from '../Authorization/Authorization.jsx';
import ErrorBoundary from '../Errors/ErrorBoundary.jsx';
import useAuthorizationClient from '../Authorization/AuthorizationClient.js';
import useCourseClient from '../Courses/CourseClient.js';
import useCountryClient from '../Locations/Countries/CountryClient.js';
import useNewClient from '../News/NewClient.js';
import usePlayerClient from '../Players/PlayerClient.js';
import useProofTypeClient from '../ProofTypes/ProofTypeClient.js';
import useSyncClient from '../Syncs/SyncClient.js';
import App from './App.jsx'
import getLocaleNameAsync from './Locale.jsx';
import { lazy, Suspense } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ReactDOM from 'react-dom/client';

const NewList = lazy(() => import('../News/NewList.jsx'));
const PlayerCreate = lazy(() => import('../Players/PlayerCreate.jsx'));
const PlayerDetail = lazy(() => import('../Players/PlayerDetail.jsx'));
const PlayerList = lazy(() => import('../Players/PlayerList.jsx'));
const RuleList = lazy(() => import('../Rules/RuleList.jsx'));
const SyncCreate = lazy(() => import('../Syncs/SyncCreate.jsx'));
const SyncDetail = lazy(() => import('../Syncs/SyncDetail.jsx'));
const TimeCreate = lazy(() => import('../Times/TimeCreate.jsx'));

const authorizationPromise = useAuthorizationClient().getAsync();
const courseClient = useCourseClient();
const countryClient = useCountryClient();
const newClient = useNewClient();
const playerClient = usePlayerClient();
const proofClient = useProofTypeClient();
const syncClient = useSyncClient();

const router = createBrowserRouter([
  {
    element: <App />,
    errorElement: <ErrorBoundary />,
    loader: getLocaleNameAsync,
    children: [
      {
        path: '/',
        element: <NewList />,
        loader: newClient.listAsync
      },
      {
        path: '/authorization/',
        element: <Authorization />
      },
      {
        path: '/news/',
        element: <NewList />,
        loader: newClient.listAsync
      },
      {
        path: '/players/',
        element: <PlayerList />,
        loader: async () => {
          const [authorization, players] = await Promise.all([
            authorizationPromise,
            playerClient.listAsync()
          ]);
          return ({
            authorization: authorization,
            players: players
          });
        }
      },
      {
        path: '/players/create/',
        element: <PlayerCreate />,
        loader: async () => {
          const [countries, proofTypes] = await Promise.all([
            countryClient.listAsync(),
            proofClient.listAsync()
          ]);
          return ({
            countries: countries,
            proofTypes: proofTypes
          });
        }
      },
      {
        path: '/players/detail/:playerName/',
        element: <PlayerDetail />,
        loader: async ({ params }) => {
          const [authorization, player] = await Promise.all([
            authorizationPromise,
            playerClient.detailAsync(params.playerName)
          ]);
          return ({
            authorization: authorization,
            player: player
          });
        }
      },
      {
        path: '/rules/',
        element: <Suspense>
          <RuleList />
        </Suspense>
      },
      {
        path: '/syncs/create/',
        element: <Suspense>
          <SyncCreate />
        </Suspense>
      },
      {
        path: '/syncs/detail/:syncId/',
        element: <SyncDetail />,
        loader: async ({ params }) => {
          return ({
            sync: await syncClient.detailAsync(params.syncId)
          });
        }
      },
      // TODO: Modularise these routes.
      // E.g. /Players/Routes.js
      {
        path: '/times/create/:playerName/:courseName/',
        element: <TimeCreate />,
        loader: async ({ params }) => {
          const [players, localeName, courses] = await Promise.all([
            playerClient.listAsync(),
            getLocaleNameAsync(),
            courseClient.listAsync()
          ]);
          return ({
            courseName: params.courseName,
            courses: courses,
            localeName: localeName,
            playerName: params.playerName,
            players: players
          });
        }
      }
    ]
  }
])

ReactDOM.createRoot(document.getElementById('root')).render(
  <RouterProvider router={router} />
);
