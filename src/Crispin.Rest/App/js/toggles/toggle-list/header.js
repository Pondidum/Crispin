import React from "react";
import Glyph from "../../util/glyph";
import Updating from "./updating";

const HeaderButton = ({ glyph, alt, handler }) => (
  <a
    className="nav-item nav-link"
    href="#"
    onClick={e => {
      e.preventDefault();
      handler();
    }}
  >
    <Glyph name={glyph} alt={alt} />
  </a>
);

const Header = ({ updating, buttons }) => (
  <div className="toggle-list-header">
    <nav className="navbar navbar-expand navbar-dark bg-dark justify-content-between ">
      <div className="navbar-nav">
        {buttons
          .filter(x => x.position === "left")
          .map((x, i) => (
            <HeaderButton key={i} {...x} />
          ))}
      </div>
      <div className="navbar-nav">
        {buttons
          .filter(x => x.position === "right")
          .map((x, i) => (
            <HeaderButton key={i} {...x} />
          ))}
      </div>
    </nav>
    <Updating updating={updating} />
  </div>
);

export default Header;
