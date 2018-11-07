import React from "react";
import { Row, Col } from "reactstrap";

import ToggleGraph from "./graph";
import TextEditor from "./text-editor";
import ConditionEditor from "./condition-editor";

const Details = ({ match }) => (
  <Row>
    <Col md="6">
      <TextEditor title="Name" value={`Toggle ${match.params.id}`} />
      <TextEditor
        title="Description"
        value="Does something very interesting and potentially has quite a long
          description"
      />
      <ConditionEditor />
    </Col>
    <Col md="6">
      <ToggleGraph title="Usage Graph" />
      <ToggleGraph title="Activation Graph" />
      <ToggleGraph title="Condition Activation Graph" />
    </Col>
  </Row>
);

export default Details;
