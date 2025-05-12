import { Box, Container } from "@mui/material";
import { Outlet } from "react-router";

import NavBar from "./NavBar";

function App() {
  return (
    <Box sx={{ bgcolor: "#eee", minHeight: "100vh" }}>
      <NavBar />
      <Container maxWidth="xl" sx={{ mt: 3 }}>
        <Outlet />
      </Container>
    </Box>
  );
}

export default App;
