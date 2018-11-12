import React from "react";
import { Col, Row } from "reactstrap";
import { Route } from "react-router-dom";

import ToggleNavigation from "./navigation";

import Details from "./details";

const Toggles = ({ match }) => (
  <Row>
    <Col sm="3" md="2" className="sidebar">
      <ToggleNavigation match={match} />
    </Col>
    <Col sm="9" md="10">
      <Route path={`${match.path}:id`} component={Details} />
    </Col>
  </Row>
);

export default Toggles;
