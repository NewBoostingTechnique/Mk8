import Authorization from '../Authorization/Authorization.jsx';
import ErrorBoundary from '../Errors/ErrorBoundary.jsx';
import useAuthorizationClient from '../Authorization/AuthorizationClient.js';
import useCourseClient from '../Courses/CourseClient.js';
import useCountryClient from '../Locations/Countries/CountryClient.js';
import usePlayerClient from '../Players/PlayerClient.js';
import useProofTypeClient from '../ProofTypes/ProofTypeClient.js';
import App from './App.jsx'
import getLocaleNameAsync from './Locale.jsx';
import { lazy } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ReactDOM from 'react-dom/client';

const NewsList = lazy(() => import('../News/NewsList.jsx'));
const PlayerCreate = lazy(() => import('../Players/PlayerCreate.jsx'));
const PlayerDetail = lazy(() => import('../Players/PlayerDetail.jsx'));
const PlayerList = lazy(() => import('../Players/PlayerList.jsx'));
const TimeCreate = lazy(() => import('../Times/TimeCreate.jsx'));

const authorizationPromise = useAuthorizationClient().getAsync();
const courseClient = useCourseClient();
const countryClient = useCountryClient();
const playerClient = usePlayerClient();
const proofClient = useProofTypeClient();

const router = createBrowserRouter([
    {
        element: <App />,
        errorElement: <ErrorBoundary />,
        loader: getLocaleNameAsync,
        children: [
            {
                path: '/',
                element: <NewsList />
            },
            {
                path: '/authorization/',
                element: <Authorization />
            },
            {
                path: '/news/',
                element: <NewsList />
            },
            {
                path: '/player/',
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
                path: '/player/create/',
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
                path: '/player/detail/:playerName/',
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
                path: '/time/create/:playerName/:courseName/',
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