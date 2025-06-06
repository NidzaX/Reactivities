import { Box, Container, CssBaseline } from '@mui/material';
import { Outlet, ScrollRestoration, useLocation } from 'react-router';

import NavBar from './NavBar';
import HomePage from '../../features/home/HomePage';

function App() {
  const location = useLocation();

  return (
    <Box sx={{ bgcolor: '#eee', minHeight: '100vh' }}>
      <ScrollRestoration />
      <CssBaseline />
      {location.pathname === '/' ? (
        <HomePage />
      ) : (
        <>
          {' '}
          <NavBar />
          <Container maxWidth="xl" sx={{ pt: 14 }}>
            <Outlet />
          </Container>
        </>
      )}
    </Box>
  );
}

export default App;
