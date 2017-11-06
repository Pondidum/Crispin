import React from "react";
import { Row, Col } from "react-bootstrap";
import Trend from "react-trend";
import ToggleState from "./toggleState";

const Toggle = ({ toggle }) => {
  const { name, description, state } = toggle;

  return (
    <div className="toggle panel panel-default col-sm-12">
      <Row>
        <Col sm={6}>
          <h3>{name}</h3>
        </Col>
        <Col sm={6}>
          <Trend
            smooth
            height={56}
            strokeWidth={2}
            gradient={["purple", "violet"]}
            data={[0, 10, 5, 22, 3.6, 11]}
          />
        </Col>
      </Row>
      <span>{description}</span>
      <ToggleState state={state} />
    </div>
  );
};

export default Toggle;
