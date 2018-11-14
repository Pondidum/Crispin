import React from "react";
import { Nav, NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";
import Glyph from "../util/Glyph";

const Navigation = () => (
  <nav className="col-md-1 navbar-dark bg-dark sidebar">
    <div className="sidebar-sticky">
      <Nav className="ml-auto" className="navbar-nav" vertical>
        <NavItem>
          <NavLink className="nav-link" to="/toggles/">
            <Glyph name="toggle-on" alt="Toggles" />
          </NavLink>
        </NavItem>
        <NavItem>
          <NavLink className="nav-link" to="/users/">
            <Glyph name="users" alt="Users" />
          </NavLink>
        </NavItem>
      </Nav>
    </div>
  </nav>
);

export default Navigation;
