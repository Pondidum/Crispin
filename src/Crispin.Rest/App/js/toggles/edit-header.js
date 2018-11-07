import React from "react";
import Glyph from "../util/glyph";

const EditHeader = ({ editing, title, startEdit, cancelEdit, acceptEdit }) => {
  const viewActions = (
    <a href="#" onClick={startEdit} className="ml-1 align-text-bottom">
      <Glyph name="pencil" />
    </a>
  );

  const editingActions = (
    <small className="d-inline float-right">
      <a href="#" onClick={cancelEdit}>
        cancel
      </a>{" "}
      <a href="#" onClick={acceptEdit}>
        ok
      </a>
    </small>
  );

  return (
    <div>
      <h4 className="d-inline">{title}</h4>
      {editing ? editingActions : viewActions}
    </div>
  );
};

export default EditHeader;
