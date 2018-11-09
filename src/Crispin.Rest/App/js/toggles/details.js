import React from "react";
import { connect } from "react-redux";
import { Row, Col } from "reactstrap";

import ToggleGraph from "./graph";
import TextEditor from "./text-editor";
import ConditionEditor from "./condition-editor";

import { updateName } from "./actions";

const mapStateToProps = (state, ownProps) => {
  const match = ownProps.match;
  return {
    toggle: state.toggles.all.find(t => t.id == match.params.id)
  };
};

const mapDispatchToProps = dispatch => {
  return {
    updateName: (id, newName) => dispatch(updateName(id, newName))
  };
};

const connector = connect(
  mapStateToProps,
  mapDispatchToProps
);

const Details = ({ toggle, updateName }) => {
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
};

export default connector(Details);
