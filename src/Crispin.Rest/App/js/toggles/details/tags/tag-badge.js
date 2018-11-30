import React from "react";
import { Badge } from "reactstrap";
import Glyph from "../../../util/glyph";

const TagBadge = ({ tag, handleRemove }) => (
  <Badge className="mx-1 p-1">
    {tag}
    <a className="ml-1 text-white" href="#" onClick={e => handleRemove(e, tag)}>
      <Glyph name="times" />
    </a>
  </Badge>
);

export default TagBadge;
