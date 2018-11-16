import { connect } from "react-redux";
import React from "react";
import { Col, Nav } from "reactstrap";
import MenuEntry from "./menu-entry";
import Updating from "./updating";
import Glyph from "../../util/glyph";
import "./toggle-list.css";

import { fetchAllToggles } from "../actions";

const mapPropsFromState = (state, ownProps) => {
  return {
    ...ownProps,
    updating: state.toggles.updating,
    toggles: state.toggles.all
  };
};

const mapDispatchToProps = dispatch => {
  return {
    refresh: () => dispatch(fetchAllToggles())
  };
};

const connector = connect(
  mapPropsFromState,
  mapDispatchToProps
);

const Navigation = ({ match, toggles, updating, refresh }) => {
  const handleRefresh = e => {
    e.preventDefault();
    refresh();
  };

  return (
    <Col sm="3" md="2" className="sidebar">
      <div className="toggle-list-header">
        <nav className="navbar navbar-expand navbar-dark bg-dark justify-content-between ">
          <div className="navbar-nav">
            <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
              <Glyph name="plus" alt="Create new Toggle" />
            </a>
          </div>
          <div className="navbar-nav">
            <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
              <Glyph name="sync" alt="Refresh toggles" />
            </a>
          </div>
        </nav>
        <Updating updating={updating} />
      </div>
      <Nav vertical className="sidebar-sticky">
        {toggles.map(t => (
          <MenuEntry key={t.id} match={match} id={t.id} name={t.name} />
        ))}
      </Nav>
    </Col>
  );
};

export default connector(Navigation);
