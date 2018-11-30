import React from "react";
import { Col } from "reactstrap";

import TagBadge from "./tag-badge";
import TagAddDialog from "./tag-add-dialog";

const TagsEditor = ({ tags, addTag, removeTag }) => {
  const handleRemove = (e, tag) => {
    e.preventDefault();
    removeTag(tag);
  };

  const badges = [...tags]
    .sort()
    .map((tag, i) => (
      <TagBadge key={i} tag={tag} handleRemove={handleRemove} />
    ));

  return (
    <Col md="12">
      <div>
        <h4 className="d-inline">Tags</h4>
      </div>
      <div className="mt-1">
        {badges}
        <TagAddDialog addTag={addTag} />
      </div>
    </Col>
  );
};

export default TagsEditor;
