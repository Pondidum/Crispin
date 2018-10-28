import React from "react";
import { Navbar, NavbarBrand, Nav, NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";

const Navigation = () => (
  <Navbar color="light" light expand="md">
    <NavLink className="navbar-brand" to="/">
      Crispin
    </NavLink>
    <Nav className="ml-auto" navbar>
      <NavItem>
        <NavLink className="nav-link" to="/toggles/">
          Toggles
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink className="nav-link" to="/users/">
          Users
        </NavLink>
      </NavItem>
    </Nav>
  </Navbar>
);

export default Navigation;
