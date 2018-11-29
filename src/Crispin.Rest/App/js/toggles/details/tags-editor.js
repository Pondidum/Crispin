import React from "react";
import { Col, Badge } from "reactstrap";
import Glyph from "../../util/glyph";

import TagAddDialog from "./tag-add-dialog";

const TagsEditor = ({ tags, addTag, removeTag }) => {
  const handleRemoveClick = (e, tag) => {
    e.preventDefault();
    removeTag(tag);
  };

  return (
    <Col md="12">
      <div>
        <h4 className="d-inline">Tags</h4>
      </div>
      <div className="mt-1">
        {tags.map((tag, i) => (
          <Badge key={i} className="mx-1 p-1">
            {tag}
            <a
              className="ml-1 text-white"
              href="#"
              onClick={e => handleRemoveClick(e, tag)}
            >
              <Glyph name="times" />
            </a>
          </Badge>
        ))}
        <TagAddDialog addTag={addTag} />
      </div>
    </Col>
  );
};

export default TagsEditor;
