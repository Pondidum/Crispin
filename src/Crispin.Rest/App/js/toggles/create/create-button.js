import React from "react";
import { Button } from "reactstrap";
import { connect } from "react-redux";
import { createToggle } from "../actions";

const mapDispatchToProps = dispatch => {
  return {
    createToggle: (name, desc) => dispatch(createToggle(name, desc))
  };
};

const CreateToggleButton = ({ createToggle, name, description, onCreate }) => {
  const onClick = () => {
    onCreate();
    createToggle(name, description);
  };

  return (
    <Button color="primary" onClick={onClick}>
      Create Toggle
    </Button>
  );
};

export default connect(
  null,
  mapDispatchToProps
)(CreateToggleButton);
