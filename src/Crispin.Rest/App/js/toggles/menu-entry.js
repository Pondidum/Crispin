import React from "react";
import { NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";

const combine = (left, right) =>
  left.endsWith("/") ? left + right : left + "/" + right;

const ToggleMenuEntry = ({ match, id }) => (
  <NavItem>
    <NavLink className="nav-link" to={combine(match.url, id)}>
      Toggle {id}
    </NavLink>
  </NavItem>
);

export default ToggleMenuEntry;
