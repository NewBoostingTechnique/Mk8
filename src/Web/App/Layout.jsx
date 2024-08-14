import BannerUrl from '../wwwroot/banner.jpg';
import Menu from '../Menu/Menu.jsx';
import CancelIcon from '@mui/icons-material/Cancel';
import MenuIcon from '@mui/icons-material/Menu';
import Image from 'mui-image'
import {
  AppBar,
  Backdrop,
  Box,
  CircularProgress,
  CssBaseline,
  Divider,
  Drawer,
  IconButton,
  Toolbar,
  Typography
} from '@mui/material';
import { blue } from '@mui/material/colors';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom'

const drawerWidth = 200;
let progressTimeout;

import PropTypes from 'prop-types';

export default function Layout({ children }) {
  Layout.propTypes = {
    children: PropTypes.node.isRequired
  };
  // Progress indicator state (open/close).
  const [isProgressShown, setIsProgressShown] = useState(false);
  function showProgress() {
    progressTimeout = setTimeout(
      () => { setIsProgressShown(true) },
      250
    );
  }
  function hideProgress() {
    clearTimeout(progressTimeout);
    setIsProgressShown(false);
  }

  // Response drawer state (open/close).
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  function openDrawer() { setIsDrawerOpen(true); }
  function closeDrawer() { setIsDrawerOpen(false); }

  // Close responsive drawer on navigate.
  const location = useLocation();
  useEffect(
    () => {
      closeDrawer();
      hideProgress();
    },
    [location]
  );

  return (
    <ThemeProvider theme={createTheme({
      palette: {
        mode: 'dark',
        primary: {
          main: blue[900]
        }
      }
    })}>
      <CssBaseline />
      <Box sx={{ display: 'flex' }}>
        <AppBar enableColorOnDark sx={{ width: { md: `calc(100% - ${drawerWidth}px)` }, ml: { md: `${drawerWidth}px` } }}>
          <Toolbar>
            <IconButton onClick={openDrawer} sx={{ mr: 2, display: { md: 'none' } }}>
              <MenuIcon />
            </IconButton>
            <Typography variant='h6'>Mario Kart 8 Players' Page</Typography>
          </Toolbar>
        </AppBar>
        <Box sx={{ width: { md: drawerWidth } }} >
          <Drawer variant="temporary" open={isDrawerOpen} onClose={closeDrawer} ModalProps={{ keepMounted: true }}
            sx={{ display: { xs: 'block', md: 'none' }, '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth } }}>
            <Toolbar sx={{ display: 'flex', alignItems: 'center', justifyContent: 'flex-end' }}>
              <IconButton>
                <CancelIcon onClick={closeDrawer} />
              </IconButton>
            </Toolbar>
            <Divider />
            <Menu showProgress={showProgress} />
          </Drawer>
          <Drawer variant="permanent" open
            sx={{ display: { xs: 'none', md: 'block' }, '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth } }}>
            <Toolbar />
            <Divider />
            <Menu showProgress={showProgress} />
          </Drawer>
        </Box>
        <Box margin="auto" sx={{ width: { md: `calc(100% - ${drawerWidth}px)` } }}>
          <Toolbar />
          <Box margin="auto" mb="1em" width={{ xs: '%100', sm: 583, md: 683, lg: 983 }} >
            <Image alt="Banner" src={BannerUrl} shift="right" sx={{ mb: '1em' }} />
            {children}
          </Box>
        </Box>
      </Box >
      <Backdrop sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }} open={isProgressShown}>
        <CircularProgress color="inherit" />
      </Backdrop>
    </ThemeProvider >
  );
}
