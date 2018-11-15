import React from "react";
import { Col, Row } from "reactstrap";
import { Route } from "react-router-dom";

import ToggleNavigation from "./navigation";

import Details from "./details";

const Toggles = ({ match }) => (
  <Row>
    <ToggleNavigation match={match} />
    <Col sm="9" md="10">
      <Route path={`${match.path}:id`} component={Details} />
    </Col>
  </Row>
);

export default Toggles;
