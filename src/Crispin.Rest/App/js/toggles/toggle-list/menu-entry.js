import React from "react";
import { NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";

const combine = (left, right) =>
  left.endsWith("/") ? left + right : left + "/" + right;

const ToggleMenuEntry = ({ match, toggle }) => (
  <NavItem>
    <NavLink className="nav-link" to={combine(match.url, toggle.id)}>
      {toggle.name}
    </NavLink>
  </NavItem>
);

export default ToggleMenuEntry;
