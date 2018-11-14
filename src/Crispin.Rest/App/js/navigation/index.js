import React from "react";
import { NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";
import Glyph from "../util/Glyph";

const Navigation = () => (
  <nav class="navbar navbar-dark bg-dark justify-content-center">
    <ul class="navbar-nav">
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
    </ul>
  </nav>
);

export default Navigation;
