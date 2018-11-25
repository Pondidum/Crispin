import React from "react";
import { connect } from "react-redux";
import { Row, Col } from "reactstrap";

import ToggleGraph from "./graph";
import TextEditor from "./text-editor";
import ConditionEditor from "./condition-editor";

import { updateName, updateDescription, changeConditionMode } from "../actions";

const mapStateToProps = (state, ownProps) => {
  const match = ownProps.match;
  return {
    toggle: state.toggles.all.find(t => t.id == match.params.id)
  };
};

const mapDispatchToProps = dispatch => {
  return {
    updateName: (id, name) => dispatch(updateName(id, name)),
    updateDescription: (id, desc) => dispatch(updateDescription(id, desc)),
    updateConditionMode: (id, mode) => dispatch(changeConditionMode(id, mode))
  };
};

const connector = connect(
  mapStateToProps,
  mapDispatchToProps
);

const Details = ({
  toggle,
  updateName,
  updateDescription,
  updateConditionMode
}) => {
  if (!toggle) {
    return <div />;
  }
  return (
    <Row>
      <Col md="6">
        <TextEditor
          title="Name"
          value={toggle.name}
          onAcceptEdit={value => updateName(toggle.id, value)}
        />
        <TextEditor
          title="Description"
          value={toggle.description}
          onAcceptEdit={value => updateDescription(toggle.id, value)}
        />
        <ConditionEditor
          conditionMode={toggle.conditionMode}
          updateConditionMode={value => updateConditionMode(toggle.id, value)}
        />
      </Col>
      <Col md="6">
        <ToggleGraph title="Usage Graph" />
        <ToggleGraph title="Activation Graph" />
        <ToggleGraph title="Condition Activation Graph" />
      </Col>
    </Row>
  );
};

export default connector(Details);
