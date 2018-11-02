import React from "react";
import { Row, Col, Button } from "reactstrap";

import Conditions from "./conditions";
import ToggleGraph from "./graph";

const Details = ({ match }) => (
  <Row>
    <Col md="6">
      <Row />
      <Col md="12">
        <h4>Name</h4>
        <p>Some Feature Toggle {match.params.id}</p>
      </Col>
      <Col md="12">
        <h4>Description</h4>
        <p>
          Does something very interesting and potentially has quite a long
          description
        </p>
      </Col>
      <Col md="12">
        <h4>Conditions</h4>
        <Conditions />
      </Col>
      <Col md="12">
        <div className="float-right">
          <Button color="link">Delete</Button>
          <Button outline color="primary">
            Edit
          </Button>
        </div>
      </Col>
    </Col>
    <Col md="6">
      <ToggleGraph title="Usage Graph" />
      <ToggleGraph title="Activation Graph" />
      <ToggleGraph title="Condition Activation Graph" />
    </Col>
  </Row>
);

export default Details;
