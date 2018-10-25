import React from "react";
import { NavLink } from "react-router-dom";

const Navigation = () => (
  <nav>
    <ul>
      <li>
        <NavLink to="/">Home</NavLink>
      </li>
      <li>
        <NavLink to="/about/">About</NavLink>
      </li>
      <li>
        <NavLink to="/users/">Users</NavLink>
      </li>
    </ul>
  </nav>
);

export default Navigation;
