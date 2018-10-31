import React from "react";
import { Nav } from "reactstrap";
import MenuEntry from "./menu-entry";

const Navigation = ({ match }) => (
  <Nav vertical>
    <MenuEntry match={match} id={1} />
    <MenuEntry match={match} id={2} />
    <MenuEntry match={match} id={3} />
    <MenuEntry match={match} id={4} />
    <MenuEntry match={match} id={5} />
    <MenuEntry match={match} id={6} />
  </Nav>
);

export default Navigation;
