import { connect } from "react-redux";
import React from "react";
import { Col, Nav } from "reactstrap";
import MenuEntry from "./menu-entry";
import Glyph from "../../util/glyph";

import { fetchAllToggles } from "../actions";

const mapPropsFromState = (state, ownProps) => {
  return {
    ...ownProps,
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

const Navigation = ({ match, toggles, refresh }) => {
  const handleRefresh = e => {
    e.preventDefault();
    refresh();
  };

  return (
    <Col sm="3" md="2" className="sidebar">
      <nav className="navbar navbar-expand navbar-dark bg-dark justify-content-end">
        <div className="navbar-nav">
          <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
            <Glyph name="sync" alt="Refresh toggles" />
          </a>
        </div>
      </nav>
      <Nav vertical className="sidebar-sticky">
        {toggles.map(t => (
          <MenuEntry key={t.id} match={match} id={t.id} name={t.name} />
        ))}
      </Nav>
    </Col>
  );
};

export default connector(Navigation);
