import React, { Component } from "react";
import { Col } from "reactstrap";

import ConditionModeEditor from "./condition-mode-editor";
import Conditions from "./conditions";

const ConditionEditor = ({ conditionMode, updateConditionMode }) => (
  <Col md="12">
    <div>
      <h4 className="d-inline">Conditions</h4>
    </div>
    <ConditionModeEditor
      conditionMode={conditionMode}
      updateConditionMode={updateConditionMode}
    />
    <Conditions />
  </Col>
);

export default ConditionEditor;
