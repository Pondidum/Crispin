import React from "react";
import { NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";
import Glyph from "../util/Glyph";

const Navigation = () => (
  <nav className="navbar navbar-dark justify-content-center">
    <ul className="navbar-nav">
      <NavItem>
        <NavLink className="nav-link" to="/toggles/">
          <Glyph name="toggle-on" alt="Toggles" />
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink className="nav-link" to="/stats/">
          <Glyph name="chart-bar" alt="Statistics" />
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink className="nav-link" to="/users/">
          <Glyph name="users" alt="Users" />
        </NavLink>
      </NavItem>
    </ul>
  </nav>
);

export default Navigation;
