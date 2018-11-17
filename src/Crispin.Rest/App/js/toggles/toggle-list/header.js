import React from "react";
import Glyph from "../../util/glyph";
import Updating from "./updating";

const Header = ({ updating, handleCreate, handleRefresh }) => (
  <div className="toggle-list-header">
    <nav className="navbar navbar-expand navbar-dark bg-dark justify-content-between ">
      <div className="navbar-nav">
        <a className="nav-item nav-link" href="#" onClick={handleCreate}>
          <Glyph name="plus" alt="Create new Toggle" />
        </a>
      </div>
      <div className="navbar-nav">
        <a className="nav-item nav-link" href="#" onClick={handleRefresh}>
          <Glyph name="sync" alt="Refresh toggles" />
        </a>
      </div>
    </nav>
    <Updating updating={updating} />
  </div>
);

export default Header;
