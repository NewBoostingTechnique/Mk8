import { lazy, Suspense } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ReactDOM from 'react-dom/client';

import useAuthorizationClient from '../Authorization/AuthorizationClient.ts';
import useCountryClient from '../Countries/CountryClient.js';
import useCourseClient from '../Courses/CourseClient.js';
import useMigrationClient from '../Migrations/MigrationClient.js';
import useNewClient from '../News/NewClient.js';
import usePlayerClient from '../Players/PlayerClient.js';

import getLocaleNameAsync from './Locale.js';
const localeNamePromise = getLocaleNameAsync();

const App = lazy(() => import('./App.jsx'));

const Authorization = lazy(() => import('../Authorization/Authorization.js'));
const authorizationPromise = useAuthorizationClient().getAsync();

const ErrorBoundary = lazy(() => import('../Errors/ErrorBoundary.js'));

const migrationClient = useMigrationClient();
const migrationUiImport = import('../Migrations/MigrationUi.jsx');
const MigrationUi = {
  Create: lazy(() => migrationUiImport.then(module => ({ default: module.Create }))),
  Detail: lazy(() => migrationUiImport.then(module => ({ default: module.Detail }))),
  Index: lazy(() => migrationUiImport.then(module => ({ default: module.Index })))
};

const newClient = useNewClient();
const newUiImport = import('../News/NewUi.jsx');
const NewUi = {
  Index: lazy(() => newUiImport.then(module => ({ default: module.Index })))
};
async function newIndexLoader() {
  const [authorization, news] = await Promise.all([
    authorizationPromise,
    newClient.indexAsync()
  ]);
  return ({
    authorization: authorization,
    news: news
  });
};

const playerClient = usePlayerClient();
const playerUiImport = import('../Players/PlayerUi.jsx');
const PlayerUi = {
  Index: lazy(() => playerUiImport.then(module => ({ default: module.Index }))),
  Create: lazy(() => playerUiImport.then(module => ({ default: module.Create }))),
  Detail: lazy(() => playerUiImport.then(module => ({ default: module.Detail })))
};
const RuleList = lazy(() => import('../Rules/RuleList.jsx'));
const CreateTime = lazy(() => import('../Times/Create/CreateTime.jsx'));

const courseClient = useCourseClient();
const countryClient = useCountryClient();

const router = createBrowserRouter([
  {
    element: <App />,
    errorElement: <ErrorBoundary />,
    loader: async () => {
      const [authorization, localeName] = await Promise.all([
        authorizationPromise,
        localeNamePromise
      ]);
      return {
        authorization: authorization,
        localeName: localeName
      }
    },
    children: [
      {
        path: '/',
        element: <NewUi.Index />,
        loader: newIndexLoader
      },
      {
        path: '/authorization/',
        element: <Authorization />
      },
      {
        path: '/migrations/',
        element: <MigrationUi.Index />,
        loader: async () => ({
          migrations: await migrationClient.indexAsync()
        })
      },
      {
        path: '/migrations/create/',
        element: <Suspense>
          <MigrationUi.Create />
        </Suspense>
      },
      {
        path: '/migrations/detail/:id/',
        element: <MigrationUi.Detail />,
        loader: async ({ params }) => {
          if (params.id == null)
            return Promise.reject(new Error("Migration id is undefined. Expected '/migrations/detail/:id'"));

          return {
            migration: await migrationClient.detailAsync(params.id)
          };
        },
      }, {
        path: '/news/',
        element: <NewUi.Index />,
        loader: newIndexLoader
      },
      {
        path: '/players/',
        element: <PlayerUi.Index />,
        loader: async () => {
          const [authorization, players] = await Promise.all([
            authorizationPromise,
            playerClient.indexAsync()
          ]);
          return ({
            authorization: authorization,
            players: players
          });
        }
      },
      {
        path: '/players/create/',
        element: <PlayerUi.Create />,
        loader: async () => {
          const [countries] = await Promise.all([
            countryClient.indexAsync()
          ]);
          return ({
            countries: countries
          });
        }
      },
      {
        path: '/players/detail/:name/',
        element: <PlayerUi.Detail />,
        loader: async ({ params }) => {
          if (params.name == null)
            return Promise.reject(new Error("Player name is undefined. Expected '/players/detail/:name'"));

          const [authorization, player] = await Promise.all([
            authorizationPromise,
            playerClient.detailAsync(params.name)
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
        path: '/times/create/:playerName/:courseName/',
        element: <CreateTime />,
        loader: async ({ params }) => {
          const [players, courses] = await Promise.all([
            playerClient.indexAsync(),
            courseClient.indexAsync()
          ]);
          return ({
            courseName: params.courseName,
            courses: courses,
            localeName: await localeNamePromise,
            playerName: params.playerName,
            players: players
          });
        }
      }
    ]
  }
])

const root = document.getElementById('root');

if (root === null)
  throw new Error("Root element not found.");

ReactDOM.createRoot(root).render(
  <RouterProvider router={router} />
);
