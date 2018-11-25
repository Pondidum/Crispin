import React from "react";

const ViewMode = () => (
  <p>
    Toggle is active when <b>any</b> conditions are true.
  </p>
);

const EditMode = () => (
  <p>
    Toggle is active when <b>any</b> | <a href="#">all</a> conditions are true.
  </p>
);

const ConditionModeEditor = ({ editing }) =>
  editing ? <EditMode /> : <ViewMode />;

export default ConditionModeEditor;
