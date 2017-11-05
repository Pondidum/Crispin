import React from "react";
import ToggleState from "./toggleState";

const Toggle = ({ toggle }) => {
  const { name, description, state } = toggle;

  return (
    <div className="toggle panel panel-default col-sm-12">
      <h3>{name}</h3>
      <span>{description}</span>
      <ToggleState state={state} />
    </div>
  );
};

export default Toggle;
