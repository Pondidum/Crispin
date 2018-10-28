import React from "react";
import { Col, Row, Nav, NavItem } from "reactstrap";
import { NavLink, Route } from "react-router-dom";

const ToggleEntry = ({ prefix, id }) => (
  <NavItem>
    <NavLink className="nav-link" to={`${prefix}/${id}`}>
      Toggle {id}
    </NavLink>
  </NavItem>
);

const SingleToggle = ({ match }) => <h3>Toggle {match.params.id}:</h3>;

const Toggles = ({ match }) => (
  <Row>
    <Col sm="3" md="2">
      <Nav vertical>
        <ToggleEntry prefix={match.url} id={1} />
        <ToggleEntry prefix={match.url} id={2} />
        <ToggleEntry prefix={match.url} id={3} />
        <ToggleEntry prefix={match.url} id={4} />
        <ToggleEntry prefix={match.url} id={5} />
        <ToggleEntry prefix={match.url} id={6} />
      </Nav>
    </Col>
    <Col sm="9" md="10">
      <Route path={`${match.path}:id`} component={SingleToggle} />
    </Col>
  </Row>
);

export default Toggles;
