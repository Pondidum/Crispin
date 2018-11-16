import React from "react";
import { Col } from "reactstrap";

import EditHeader from "./edit-header";
import Conditions from "./conditions";

const ConditionEditor = () => (
  <Col md="12">
    <EditHeader title="Conditions" startEdit={() => {}} />
    <Conditions />
  </Col>
);

export default ConditionEditor;
