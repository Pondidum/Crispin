import React from "react";
import "@fortawesome/fontawesome-free/css/all.css";

const Glyph = ({ name, alt }) => (
  <span title={alt} className={"glyph fas fa-" + name} />
);
export default Glyph;
