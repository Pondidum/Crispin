import React from "react";
import { connect } from "react-redux";
import { Row, Col } from "reactstrap";

import ToggleGraph from "./graph";
import TextEditor from "./text-editor";
import ConditionEditor from "./condition-editor";

const mapStateToProps = (state, ownProps) => {
  const match = ownProps.match;
  return {
    toggle: state.toggles.all.find(t => t.id == match.params.id)
  };
};

const Protect = ({ toggle }) => (toggle ? <Details toggle={toggle} /> : null);

const Details = ({ toggle }) => (
  <Row>
    <Col md="6">
      <TextEditor title="Name" value={toggle.name} />
      <TextEditor title="Description" value={toggle.description} />
      <ConditionEditor />
    </Col>
    <Col md="6">
      <ToggleGraph title="Usage Graph" />
      <ToggleGraph title="Activation Graph" />
      <ToggleGraph title="Condition Activation Graph" />
    </Col>
  </Row>
);

export default connect(mapStateToProps)(Protect);
