import React from "react";
import { Col, Row, Nav } from "reactstrap";
import { Route } from "react-router-dom";

import MenuEntry from "./menu-entry";
import Details from "./details";

const Toggles = ({ match }) => (
  <Row>
    <Col sm="3" md="2">
      <Nav vertical>
        <MenuEntry match={match} id={1} />
        <MenuEntry match={match} id={2} />
        <MenuEntry match={match} id={3} />
        <MenuEntry match={match} id={4} />
        <MenuEntry match={match} id={5} />
        <MenuEntry match={match} id={6} />
      </Nav>
    </Col>
    <Col sm="9" md="10">
      <Route path={`${match.path}:id`} component={Details} />
    </Col>
  </Row>
);

export default Toggles;
