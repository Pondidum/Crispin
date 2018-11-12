import React from "react";
import { Nav, NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";

const Navigation = () => (
  <nav className="col-md-1 navbar-dark bg-dark sidebar">
    <div className="sidebar-sticky">
      <Nav className="ml-auto" className="navbar-nav" vertical>
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
    </div>
  </nav>
);

export default Navigation;
