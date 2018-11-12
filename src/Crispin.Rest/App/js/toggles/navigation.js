import { connect } from "react-redux";
import React from "react";
import { Nav } from "reactstrap";
import MenuEntry from "./menu-entry";

const mapPropsFromState = (state, ownProps) => {
  return {
    ...ownProps,
    toggles: state.toggles.all
  };
};

const Navigation = ({ match, toggles }) => (
  <Nav vertical className="sidebar-sticky">
    {toggles.map(t => (
      <MenuEntry key={t.id} match={match} id={t.id} name={t.name} />
    ))}
  </Nav>
);

export default connect(mapPropsFromState)(Navigation);
