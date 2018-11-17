import React from "react";
import { Input } from "reactstrap";

const Filter = ({ onFilterChanged }) => (
  <Input
    bsSize="sm"
    placeholder="filter..."
    onChange={e => onFilterChanged(e.target.value.toLowerCase())}
  />
);

export default Filter;
