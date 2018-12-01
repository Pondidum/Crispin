import React from "react";

const Selection = ({ isActive, value, onClick }) =>
  isActive ? (
    <b>{value}</b>
  ) : (
    <a href="#" onClick={onClick}>
      {value}
    </a>
  );

const ConditionModeEditor = ({ conditionMode, updateConditionMode }) => (
  <p>
    Toggle is active when{" "}
    <Selection
      isActive={conditionMode === "any"}
      value="any"
      onClick={() => updateConditionMode("any")}
    />{" "}
    |{" "}
    <Selection
      isActive={conditionMode === "all"}
      value="all"
      onClick={() => updateConditionMode("all")}
    />{" "}
    conditions are true.
  </p>
);

export default ConditionModeEditor;
