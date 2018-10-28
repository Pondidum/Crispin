import React from "react";
import { NavItem } from "reactstrap";
import { NavLink } from "react-router-dom";

const ToggleMenuEntry = ({ match, id }) => (
  <NavItem>
    <NavLink className="nav-link" to={`${match.url}/${id}`}>
      Toggle {id}
    </NavLink>
  </NavItem>
);

export default ToggleMenuEntry;
