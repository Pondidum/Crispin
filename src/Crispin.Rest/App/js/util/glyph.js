import React from "react";
import url from "../../img/open-iconic.min.svg";
import "./glyph.css";

const Glyph = ({ name }) => (
  <svg viewBox="0 0 8 8" className="glyph">
    <use href={url + "#" + name} />
  </svg>
);

export default Glyph;
